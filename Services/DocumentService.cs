using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;
using System.Security.Cryptography;

namespace InsurancePolicyManagement.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _repo;
        private readonly IUnderwriterService _underwriterService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public DocumentService(IDocumentRepository repo, IUnderwriterService underwriterService, IMapper mapper, IWebHostEnvironment environment)
        {
            _repo = repo;
            _underwriterService = underwriterService;
            _mapper = mapper;
            _environment = environment;
        }

        public async Task<DocumentDto?> UploadAsync(DocumentUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0) return null;

            // Validate file type
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx" };
            var fileExtension = Path.GetExtension(dto.File.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension)) return null;

            // Clean empty string IDs and Swagger default "string" values to null
            var proposalId = !string.IsNullOrEmpty(dto.ProposalId) ? dto.ProposalId : null;
            var policyId = !string.IsNullOrEmpty(dto.PolicyId) ? dto.PolicyId : null;
            var claimId = !string.IsNullOrEmpty(dto.ClaimId) ? dto.ClaimId : null;
            var userId = dto.UserId;

            // Create upload directory
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "documents");
            Directory.CreateDirectory(uploadPath);

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Calculate file hash
            var fileHash = await CalculateFileHashAsync(filePath);

            var document = new Document
            {
                UserId = userId,
                ProposalId = proposalId,
                PolicyId = policyId,
                ClaimId = claimId,
                DocumentType = DetermineDocumentType(dto, fileExtension),
                FileName = dto.File.FileName,
                FilePath = filePath,
                FileHash = fileHash,
                Status = Status.Uploaded
            };

            var created = await _repo.AddAsync(document);
            return _mapper.Map<DocumentDto>(created);
        }

        public async Task<IEnumerable<DocumentDto>> GetByUserIdAsync(string userId)
        {
            var documents = await _repo.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<IEnumerable<DocumentDto>> GetByProposalIdAsync(string proposalId)
        {
            var documents = await _repo.GetByProposalIdAsync(proposalId);
            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<IEnumerable<DocumentDto>> GetByPolicyIdAsync(string policyId)
        {
            var documents = await _repo.GetByPolicyIdAsync(policyId);
            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<IEnumerable<DocumentDto>> GetByClaimIdAsync(string claimId)
        {
            var documents = await _repo.GetByClaimIdAsync(claimId);
            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<IEnumerable<DocumentDto>> GetPendingVerificationAsync()
        {
            var documents = await _repo.GetPendingVerificationAsync();
            return _mapper.Map<IEnumerable<DocumentDto>>(documents);
        }

        public async Task<DocumentDto?> GetByIdAsync(string id)
        {
            var document = await _repo.GetByIdAsync(id);
            return document == null ? null : _mapper.Map<DocumentDto>(document);
        }

        public async Task<byte[]?> DownloadAsync(string id)
        {
            var document = await _repo.GetByIdAsync(id);
            if (document == null || !File.Exists(document.FilePath)) return null;

            return await File.ReadAllBytesAsync(document.FilePath);
        }

        public async Task<bool> DeleteAsync(string id, string userId)
        {
            var document = await _repo.GetByIdAsync(id);
            if (document == null || document.UserId != userId) return false;

            // Delete physical file
            if (File.Exists(document.FilePath))
            {
                File.Delete(document.FilePath);
            }

            return await _repo.DeleteAsync(id);
        }



        private static DocumentType DetermineDocumentType(DocumentUploadDto dto, string fileExtension)
        {
           
            if (!string.IsNullOrEmpty(dto.ClaimId))
            {
                return fileExtension.ToLower() switch
                {
                    ".pdf" => DocumentType.DeathCertificate,
                    ".jpg" or ".jpeg" or ".png" => DocumentType.Photo,
                    ".doc" or ".docx" => DocumentType.Medical,
                    _ => DocumentType.Identity
                };
            }
            
            // For Proposals - Identity/Income documents
            if (!string.IsNullOrEmpty(dto.ProposalId))
            {
                return fileExtension.ToLower() switch
                {
                    ".pdf" => DocumentType.Income,
                    ".jpg" or ".jpeg" or ".png" => DocumentType.Medical,
                    ".doc" or ".docx" => DocumentType.Income,
                    _ => DocumentType.Medical
                };
            }
            
           
            if (!string.IsNullOrEmpty(dto.PolicyId))
            {
                return DocumentType.PolicyDocument;
            }
            
           
            return DocumentType.Identity;
        }

        public async Task<KYCUploadResponseDto?> UploadKYCDocumentsAsync(KYCDocumentUploadDto dto)
        {
            var uploadedDocs = new List<string>();
            var uploadPath = Path.Combine(_environment.WebRootPath, "uploads", "kyc");
            Directory.CreateDirectory(uploadPath);

           
            if (dto.AadhaarFile != null)
            {
                var aadhaarDoc = await UploadKYCFile(dto.AadhaarFile, dto.UserId, DocumentType.Aadhaar, uploadPath);
                if (aadhaarDoc != null) uploadedDocs.Add("Aadhaar");
            }

            
            if (dto.PANFile != null)
            {
                var panDoc = await UploadKYCFile(dto.PANFile, dto.UserId, DocumentType.PAN, uploadPath);
                if (panDoc != null) uploadedDocs.Add("PAN");
            }

          
            if (!string.IsNullOrEmpty(dto.BankName) && !string.IsNullOrEmpty(dto.AccountNumber))
            {
                var bankDetails = $"Bank: {dto.BankName}\nAccount: {dto.AccountNumber}\nIFSC: {dto.IFSCCode}\nHolder: {dto.AccountHolderName}";
                var bankDoc = await CreateBankDetailsDocument(dto.UserId, bankDetails, uploadPath);
                if (bankDoc != null) uploadedDocs.Add("Bank Details");
            }

            return new KYCUploadResponseDto
            {
                Message = $"KYC documents uploaded successfully. {uploadedDocs.Count} documents processed.",
                UploadedDocuments = uploadedDocs,
                KYCStatus = "Pending"
            };
        }

        private async Task<Document?> UploadKYCFile(IFormFile file, string userId, DocumentType docType, string uploadPath)
        {
            var allowedExtensions = new[] { ".pdf", ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(fileExtension)) return null;

            var fileName = $"{userId}_{docType}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new Document
            {
                UserId = userId,
                DocumentType = docType,
                FileName = file.FileName,
                FilePath = filePath,
                FileHash = await CalculateFileHashAsync(filePath),
                Status = Status.Uploaded
            };

            return await _repo.AddAsync(document);
        }

        private async Task<Document?> CreateBankDetailsDocument(string userId, string bankDetails, string uploadPath)
        {
            var fileName = $"{userId}_BankDetails_{Guid.NewGuid()}.txt";
            var filePath = Path.Combine(uploadPath, fileName);

            await File.WriteAllTextAsync(filePath, bankDetails);

            var document = new Document
            {
                UserId = userId,
                DocumentType = DocumentType.BankStatement,
                FileName = "Bank_Details.txt",
                FilePath = filePath,
                FileHash = await CalculateFileHashAsync(filePath),
                Status = Status.Uploaded
            };

            return await _repo.AddAsync(document);
        }

        public async Task<bool> VerifyDocumentAsync(string documentId, bool isVerified, string verificationNotes)
        {
            Console.WriteLine($"=== DOCUMENT SERVICE VERIFYING DOCUMENT {documentId} ===");
            Console.WriteLine($"IsVerified: {isVerified}");
            
            var document = await _repo.GetByIdAsync(documentId);
            if (document == null)
            {
                Console.WriteLine($"Document {documentId} not found!");
                return false;
            }

            Console.WriteLine($"Document found: {document.DocumentId}, Type: {document.DocumentType}, Current Status: {document.Status}");
            Console.WriteLine($"Document ProposalId: '{document.ProposalId}'");

            var newStatus = isVerified ? Status.Verified : Status.Rejected;
            Console.WriteLine($"Updating document status from {document.Status} to {newStatus}");
            
            document.Status = newStatus;
            document.VerificationNotes = verificationNotes;
            document.VerifiedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(document);
            if (updated == null)
            {
                Console.WriteLine($"Failed to update document {documentId}");
                return false;
            }

            Console.WriteLine($"Document {documentId} updated successfully");

            // Check if all user documents are verified and update KYC status
            await CheckAndUpdateKYCStatusAsync(document.UserId);

            // Check if this document belongs to a proposal and auto-issue if all documents verified
            if (!string.IsNullOrEmpty(document.ProposalId) && isVerified)
            {
                Console.WriteLine($"Document belongs to proposal {document.ProposalId} and is verified - triggering auto-issue check");
                await CheckAndAutoIssueProposalAsync(document.ProposalId);
            }
            else
            {
                Console.WriteLine($"Auto-issue not triggered: ProposalId='{document.ProposalId}', IsVerified={isVerified}");
            }

            Console.WriteLine($"=== DOCUMENT SERVICE VERIFICATION COMPLETE ===");
            return true;
        }

        private async Task CheckAndAutoIssueProposalAsync(string proposalId)
        {
            try
            {
                Console.WriteLine($"Checking auto-issue for proposal: {proposalId}");
                
                // Get the proposal using the repository directly
                var proposal = await _repo.GetProposalByIdAsync(proposalId);
                if (proposal == null)
                {
                    Console.WriteLine($"Proposal {proposalId} not found");
                    return;
                }
                
                Console.WriteLine($"Proposal {proposalId} status: {proposal.Status}");
                if (proposal.Status != Status.Approved && proposal.Status != Status.Applied && proposal.Status != Status.UnderReview)
                {
                    Console.WriteLine($"Proposal {proposalId} not in valid status for auto-issue (must be Approved, Applied, or UnderReview), skipping");
                    return;
                }

                // Get all documents for this proposal
                var proposalDocuments = await _repo.GetByProposalIdAsync(proposalId);
                var docList = proposalDocuments.ToList();

                Console.WriteLine($"Found {docList.Count} documents for proposal {proposalId}");
                foreach (var doc in docList)
                {
                    Console.WriteLine($"  Document {doc.DocumentId}: Type={doc.DocumentType}, Status={doc.Status}");
                }

                // Check if we have at least 3 documents (Medical, Income, Identity)
                if (docList.Count < 3)
                {
                    Console.WriteLine($"Only {docList.Count} documents, need at least 3");
                    return;
                }

                // Check if ALL documents are verified
                var verifiedCount = docList.Count(d => d.Status == Status.Verified);
                Console.WriteLine($"Verified documents: {verifiedCount}/{docList.Count}");
                
                if (docList.All(d => d.Status == Status.Verified))
                {
                    // Auto-issue the proposal
                    proposal.Status = Status.Issued;
                    proposal.ReviewedDate = DateTime.UtcNow;
                    proposal.UnderwritingNotes = "Auto-issued after all documents verified";
                    
                    var updatedProposal = await _repo.UpdateProposalAsync(proposal);
                    if (updatedProposal != null)
                    {
                        Console.WriteLine($"SUCCESS: Proposal {proposalId} automatically issued!");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to update proposal {proposalId} status");
                    }
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

        public async Task<IEnumerable<DocumentDto>> GetAllForVerificationAsync()
        {
            var documents = await _repo.GetAllAsync();
            var verificationDocs = documents.Where(d => 
                d.Status == Status.Pending || 
                d.Status == Status.Verified || 
                d.Status == Status.Rejected);
            return _mapper.Map<IEnumerable<DocumentDto>>(verificationDocs);
        }

        public async Task<bool> RefreshKYCStatusAsync(string userId)
        {
            try
            {
                await CheckAndUpdateKYCStatusAsync(userId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task CheckAndUpdateKYCStatusAsync(string userId)
        {
            // Get all user documents
            var userDocuments = await _repo.GetByUserIdAsync(userId);
            
            // Check if user has KYC documents (Aadhaar, PAN, BankStatement)
            var kycDocTypes = new[] { DocumentType.Aadhaar, DocumentType.PAN, DocumentType.BankStatement };
            var kycDocs = userDocuments.Where(d => kycDocTypes.Contains(d.DocumentType)).ToList();
            
            if (kycDocs.Count == 0) return; // No KYC documents uploaded yet
            
            // Check if all KYC documents are verified
            var allKycVerified = kycDocs.All(d => d.Status == Status.Verified);
            var anyKycRejected = kycDocs.Any(d => d.Status == Status.Rejected);
            
            // Determine KYC status and notes
            string kycStatus;
            string notes;
            
            if (anyKycRejected)
            {
                kycStatus = "Rejected";
                notes = "One or more KYC documents were rejected";
            }
            else if (allKycVerified)
            {
                kycStatus = "Verified";
                notes = "All KYC documents verified successfully";
            }
            else
            {
                kycStatus = "Pending";
                notes = "KYC verification in progress";
            }
            
            // Use existing UnderwriterService method to update KYC status
            await _underwriterService.UpdateKYCStatusAsync(userId, kycStatus, notes);
        }

        private static async Task<string> CalculateFileHashAsync(string filePath)
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            var hash = await Task.Run(() => sha256.ComputeHash(stream));
            return Convert.ToBase64String(hash);
        }
    }
}
