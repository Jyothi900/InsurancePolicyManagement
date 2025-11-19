using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly InsuranceManagementContext _context;

        public DocumentRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Document>> GetAllAsync()
        {
            return await _context.Documents
                .Include(d => d.User)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<Document?> GetByIdAsync(string id)
        {
            return await _context.Documents
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DocumentId == id);
        }

        public async Task<IEnumerable<Document>> GetByUserIdAsync(string userId)
        {
            return await _context.Documents
                .Where(d => d.UserId == userId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByProposalIdAsync(string proposalId)
        {
            return await _context.Documents
                .Where(d => d.ProposalId == proposalId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByPolicyIdAsync(string policyId)
        {
            return await _context.Documents
                .Where(d => d.PolicyId == policyId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByClaimIdAsync(string claimId)
        {
            return await _context.Documents
                .Where(d => d.ClaimId == claimId)
                .OrderByDescending(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetPendingVerificationAsync()
        {
            return await _context.Documents
                .Include(d => d.User)
                .Where(d => d.Status == Status.Uploaded || d.Status == Status.Pending)
                .OrderBy(d => d.UploadedAt)
                .ToListAsync();
        }

        public async Task<Document> AddAsync(Document document)
        {
            var lastDocument = await _context.Documents
                                     .OrderByDescending(d => d.DocumentId)
                                     .FirstOrDefaultAsync();

            string newId = lastDocument == null
                ? "DOC001"
                : "DOC" + (int.Parse(lastDocument.DocumentId.Substring(3)) + 1).ToString("D3");

            document.DocumentId = newId;
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<Document> UpdateAsync(Document document)
        {
            _context.Documents.Update(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == id);
            if (document == null) return false;

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(string documentId, Status status, string? notes = null)
        {
            var document = await _context.Documents.FirstOrDefaultAsync(d => d.DocumentId == documentId);
            if (document == null) return false;

            document.Status = status;
            document.VerificationNotes = notes;
            document.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Proposal?> GetProposalByIdAsync(string proposalId)
        {
            return await _context.Proposals.FirstOrDefaultAsync(p => p.ProposalId == proposalId);
        }

        public async Task<Proposal?> UpdateProposalAsync(Proposal proposal)
        {
            _context.Proposals.Update(proposal);
            await _context.SaveChangesAsync();
            return proposal;
        }
    }
}
