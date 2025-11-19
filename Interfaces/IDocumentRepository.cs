using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IDocumentRepository
    {
        Task<IEnumerable<Document>> GetAllAsync();
        Task<Document?> GetByIdAsync(string id);
        Task<IEnumerable<Document>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Document>> GetByProposalIdAsync(string proposalId);
        Task<IEnumerable<Document>> GetByPolicyIdAsync(string policyId);
        Task<IEnumerable<Document>> GetByClaimIdAsync(string claimId);
        Task<IEnumerable<Document>> GetPendingVerificationAsync();
        Task<Document> AddAsync(Document document);
        Task<Document> UpdateAsync(Document document);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string documentId, Status status, string? notes = null);
        Task<Proposal?> GetProposalByIdAsync(string proposalId);
        Task<Proposal?> UpdateProposalAsync(Proposal proposal);
    }
}
