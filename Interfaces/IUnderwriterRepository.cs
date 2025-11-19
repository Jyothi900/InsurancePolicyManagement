using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IUnderwriterRepository
    {
        Task<IEnumerable<UnderwritingCase>> GetPendingCasesAsync(string underwriterId);
        Task<UnderwritingCase> ProcessCaseAsync(UnderwritingCase underwritingCase);
        Task<IEnumerable<DocumentVerification>> GetPendingDocumentsAsync(string underwriterId);
        Task<IEnumerable<DocumentVerification>> GetPendingDocumentsByUnderwriterAsync(string underwriterId);
        Task<IEnumerable<Document>> GetAllPendingDocumentsAsync();
        Task<IEnumerable<Proposal>> GetPendingProposalsAsync();
        Task<bool> UpdateKYCStatusAsync(string userId, string status, string? notes = null);
        Task<bool> UpdateProposalStatusAsync(string proposalId, string status, string? notes = null);
        Task<DocumentVerification> VerifyDocumentAsync(DocumentVerification verification);
        Task<IEnumerable<User>> GetAssignedCustomersAsync(string underwriterId);
        Task<bool> CanIssueProposalAsync(string proposalId);
        Task<IEnumerable<Proposal>> GetApprovedProposalsReadyForIssuanceAsync();

    }
}
