using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IUnderwriterService
    {
        Task<IEnumerable<UnderwritingCaseDto>> GetPendingCasesAsync(string underwriterId);
        Task<UnderwritingCaseDto> ProcessCaseAsync(string caseId, string decision, string reason, decimal? approvedSumAssured = null);
        Task<IEnumerable<DocumentDto>> GetPendingDocumentsAsync(string underwriterId);
        Task<IEnumerable<DocumentDto>> GetPendingDocumentsByUnderwriterAsync(string underwriterId);
        Task<IEnumerable<DocumentDto>> GetAllPendingDocumentsAsync();
        Task<IEnumerable<ProposalDto>> GetPendingProposalsAsync();
        Task<DocumentDto> VerifyDocumentAsync(string verificationId, bool isVerified, string notes);

        Task<bool> UpdateKYCStatusAsync(string userId, string status, string? notes = null);
        Task<bool> UpdateProposalStatusAsync(string proposalId, string status, string? notes = null);
        Task<IEnumerable<AssignedCustomerDto>> GetAssignedCustomersAsync(string underwriterId);
    }
}
