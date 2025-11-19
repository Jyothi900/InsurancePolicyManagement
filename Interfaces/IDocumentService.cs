using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentDto?> UploadAsync(DocumentUploadDto dto);
        Task<IEnumerable<DocumentDto>> GetByUserIdAsync(string userId);
        Task<IEnumerable<DocumentDto>> GetByProposalIdAsync(string proposalId);
        Task<IEnumerable<DocumentDto>> GetByPolicyIdAsync(string policyId);
        Task<IEnumerable<DocumentDto>> GetByClaimIdAsync(string claimId);
        Task<IEnumerable<DocumentDto>> GetPendingVerificationAsync();
        Task<DocumentDto?> GetByIdAsync(string id);
        Task<byte[]?> DownloadAsync(string id);
        Task<bool> DeleteAsync(string id, string userId);

        Task<KYCUploadResponseDto?> UploadKYCDocumentsAsync(KYCDocumentUploadDto dto);
        Task<bool> VerifyDocumentAsync(string documentId, bool isVerified, string verificationNotes);
        Task<IEnumerable<DocumentDto>> GetAllForVerificationAsync();
        Task<bool> RefreshKYCStatusAsync(string userId);
    }
}
