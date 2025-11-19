using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyManagement.Repository
{
    public class UnderwriterRepository : IUnderwriterRepository
    {
        private readonly InsuranceManagementContext _context;

        public UnderwriterRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnderwritingCase>> GetPendingCasesAsync(string underwriterId)
        {
            try
            {
                return await _context.UnderwritingCases
                    .Where(c => c.AssignedUnderwriterId == underwriterId && c.Status == "Assigned")
                    .Include(c => c.User)
                    .Take(50) // Add pagination limit
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<UnderwritingCase>();
            }
        }

        public async Task<UnderwritingCase> ProcessCaseAsync(UnderwritingCase underwritingCase)
        {
            try
            {
                _context.UnderwritingCases.Update(underwritingCase);
                await _context.SaveChangesAsync();
                return underwritingCase;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to process underwriting case");
            }
        }

        public async Task<IEnumerable<DocumentVerification>> GetPendingDocumentsAsync(string underwriterId)
        {
            // Get all pending documents (not filtered by underwriter for backward compatibility)
            var documents = await _context.Documents
                .Include(d => d.User)
                .Where(d => d.Status == Status.Uploaded || d.Status == Status.Pending)
                .Take(50) // Add limit to prevent timeout
                .ToListAsync();

            // Convert to DocumentVerification format
            return documents.Select(doc => new DocumentVerification
            {
                VerificationId = doc.DocumentId,
                DocumentId = doc.DocumentId,
                VerificationStatus = "Pending",
                Document = doc
            }).ToList();
        }

        public async Task<DocumentVerification> VerifyDocumentAsync(DocumentVerification verification)
        {
            try
            {
                Console.WriteLine($"=== VERIFYING DOCUMENT {verification.DocumentId} ===");
                Console.WriteLine($"IsVerified: {verification.IsVerified}");
                
                // Find the actual document and update its status
                var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == verification.DocumentId);
                if (document == null)
                {
                    Console.WriteLine($"Document {verification.DocumentId} not found!");
                    throw new InvalidOperationException("Document not found");
                }

                Console.WriteLine($"Document found: {document.DocumentId}, Type: {document.DocumentType}, Current Status: {document.Status}");
                Console.WriteLine($"Document ProposalId: '{document.ProposalId}'");

                // Update document status based on verification result
                var newStatus = verification.IsVerified ? Status.Verified : Status.Rejected;
                Console.WriteLine($"Updating document status from {document.Status} to {newStatus}");
                document.Status = newStatus;
                
                // Create or update the verification record
                var existingVerification = await _context.DocumentVerifications
                    .FirstOrDefaultAsync(dv => dv.DocumentId == verification.DocumentId);
                
                if (existingVerification != null)
                {
                    Console.WriteLine($"Updating existing verification record");
                    existingVerification.IsVerified = verification.IsVerified;
                    existingVerification.VerificationNotes = verification.VerificationNotes;
                    existingVerification.VerificationStatus = verification.VerificationStatus;
                    existingVerification.VerifiedAt = DateTime.Now;
                }
                else
                {
                    Console.WriteLine($"Creating new verification record");
                    // Generate new verification ID
                    var lastVerification = await _context.DocumentVerifications
                        .OrderByDescending(dv => dv.VerificationId)
                        .FirstOrDefaultAsync();
                    
                    string newVerificationId = lastVerification == null
                        ? "VER001"
                        : "VER" + (int.Parse(lastVerification.VerificationId.Substring(3)) + 1).ToString("D3");
                    
                    verification.VerificationId = newVerificationId;
                    verification.VerifiedAt = DateTime.Now;
                    _context.DocumentVerifications.Add(verification);
                }
                
                Console.WriteLine($"Saving changes to database...");
                await _context.SaveChangesAsync();
                Console.WriteLine($"Changes saved successfully");
                
                // Check if this document belongs to a proposal and auto-issue if all documents verified
                if (!string.IsNullOrEmpty(document.ProposalId) && verification.IsVerified)
                {
                    Console.WriteLine($"Document belongs to proposal {document.ProposalId} and is verified - triggering auto-issue check");
                    await CheckAndAutoIssueProposalAsync(document.ProposalId);
                }
                else
                {
                    Console.WriteLine($"Auto-issue not triggered: ProposalId='{document.ProposalId}', IsVerified={verification.IsVerified}");
                }
                
                // Return the verification with the updated document
                verification.Document = document;
                Console.WriteLine($"=== DOCUMENT VERIFICATION COMPLETE ===");
                return verification;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in VerifyDocumentAsync: {ex.Message}");
                throw new InvalidOperationException("Failed to verify document");
            }
        }

        private async Task CheckAndAutoIssueProposalAsync(string proposalId)
        {
            try
            {
                Console.WriteLine($"Checking auto-issue for proposal: {proposalId}");
                
                // Get the proposal
                var proposal = await _context.Proposals.FirstOrDefaultAsync(p => p.ProposalId == proposalId);
                if (proposal == null)
                {
                    Console.WriteLine($"Proposal {proposalId} not found");
                    return;
                }
                
                Console.WriteLine($"Proposal {proposalId} status: {proposal.Status}");
                if (proposal.Status != Status.Approved && proposal.Status != Status.Applied && proposal.Status != Status.UnderReview)
                {
                    Console.WriteLine($"Proposal {proposalId} not in valid status for auto-issue, skipping");
                    return;
                }

                // Get all documents for this proposal
                var proposalDocuments = await _context.Documents
                    .Where(d => d.ProposalId == proposalId)
                    .ToListAsync();

                Console.WriteLine($"Found {proposalDocuments.Count} documents for proposal {proposalId}");
                foreach (var doc in proposalDocuments)
                {
                    Console.WriteLine($"  Document {doc.DocumentId}: Type={doc.DocumentType}, Status={doc.Status}");
                }

                // Check if we have at least 3 documents (Medical, Income, Identity)
                if (proposalDocuments.Count < 3)
                {
                    Console.WriteLine($"Only {proposalDocuments.Count} documents, need at least 3");
                    return;
                }

                // Check if ALL documents are verified
                var verifiedCount = proposalDocuments.Count(d => d.Status == Status.Verified);
                Console.WriteLine($"Verified documents: {verifiedCount}/{proposalDocuments.Count}");
                
                if (proposalDocuments.All(d => d.Status == Status.Verified))
                {
                    // Auto-issue the proposal
                    proposal.Status = Status.Issued;
                    proposal.ReviewedDate = DateTime.Now;
                    proposal.UnderwritingNotes = "Auto-issued after all documents verified";
                    
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"SUCCESS: Proposal {proposalId} automatically issued!");
                }
                else
                {
                    Console.WriteLine($"Not all documents verified yet for proposal {proposalId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in auto-issue check for proposal {proposalId}: {ex.Message}");
            }
        }



        public async Task<IEnumerable<DocumentVerification>> GetPendingDocumentsByUnderwriterAsync(string underwriterId)
        {
            // Get ALL pending documents (not filtered by underwriter assignment)
            // This ensures all uploaded documents are visible to underwriters
            var documents = await _context.Documents
                .Include(d => d.User)
                .Where(d => d.Status == Status.Uploaded || d.Status == Status.Pending)
                .Take(50) // Add limit to prevent timeout
                .ToListAsync();

            // Convert to DocumentVerification format for compatibility
            return documents.Select(doc => new DocumentVerification
            {
                VerificationId = doc.DocumentId,
                DocumentId = doc.DocumentId,
                VerificationStatus = "Pending",
                Document = doc
            }).ToList();
        }

        public async Task<bool> UpdateKYCStatusAsync(string userId, string status, string? notes = null)
        {
            try
            {
                if (!Enum.TryParse<KYCStatus>(status, out var kycStatus))
                    return false;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                if (user == null) return false;

                user.KYCStatus = kycStatus;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<User>> GetAssignedCustomersAsync(string underwriterId)
        {
            try
            {
                return await _context.Users
                    .Where(u => u.AssignedUnderwriterId == underwriterId && u.Role == UserRole.Customer)
                    .Include(u => u.Documents)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<User>();
            }
        }

        public async Task<IEnumerable<Document>> GetAllPendingDocumentsAsync()
        {
            try
            {
                return await _context.Documents
                    .Where(d => d.Status == Status.Uploaded || d.Status == Status.Pending)
                    .Take(100) // Limit to prevent timeout
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<Document>();
            }
        }

        public async Task<IEnumerable<Proposal>> GetPendingProposalsAsync()
        {
            try
            {
                return await _context.Proposals
                    .Where(p => p.Status == Status.Pending || p.Status == Status.UnderReview || p.Status == Status.Applied)
                    .Take(50)
                    .ToListAsync();
            }
            catch (Exception)
            {
                return new List<Proposal>();
            }
        }

        public async Task<bool> UpdateProposalStatusAsync(string proposalId, string status, string? notes = null)
        {
            try
            {
                if (!Enum.TryParse<Status>(status, true, out var proposalStatus))
                    return false;

                var proposal = await _context.Proposals.FirstOrDefaultAsync(p => p.ProposalId == proposalId);
                if (proposal == null)
                    return false;
                
                proposal.Status = proposalStatus;
                proposal.ReviewedDate = DateTime.Now;
                if (!string.IsNullOrEmpty(notes))
                {
                    proposal.UnderwritingNotes = notes;
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CanIssueProposalAsync(string proposalId)
        {
            try
            {
                var proposal = await _context.Proposals.FirstOrDefaultAsync(p => p.ProposalId == proposalId);
                if (proposal == null || (proposal.Status != Status.Approved && proposal.Status != Status.Applied && proposal.Status != Status.UnderReview))
                    return false;

                // Check if all required documents for this proposal are verified
                var proposalDocuments = await _context.Documents
                    .Where(d => d.ProposalId == proposalId)
                    .ToListAsync();

                if (!proposalDocuments.Any())
                    return false; // No documents uploaded

                // All documents must be verified
                return proposalDocuments.All(d => d.Status == Status.Verified);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Proposal>> GetApprovedProposalsReadyForIssuanceAsync()
        {
            try
            {
                var approvedProposals = await _context.Proposals
                    .Where(p => p.Status == Status.Approved)
                    .Include(p => p.Documents)
                    .ToListAsync();

                var readyForIssuance = new List<Proposal>();
                
                foreach (var proposal in approvedProposals)
                {
                    if (proposal.Documents != null && proposal.Documents.Any() && 
                        proposal.Documents.All(d => d.Status == Status.Verified))
                    {
                        readyForIssuance.Add(proposal);
                    }
                }

                return readyForIssuance;
            }
            catch (Exception)
            {
                return new List<Proposal>();
            }
        }
    }
}
