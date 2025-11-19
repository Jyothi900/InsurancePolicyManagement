using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IClaimService
    {
        Task<ClaimDto?> FileClaimAsync(string userId, ClaimCreateDto dto);
        Task<IEnumerable<ClaimDto>> GetAllAsync();
        Task<IEnumerable<ClaimDto>> GetByUserIdAsync(string userId);
        Task<ClaimStatusDto?> GetStatusAsync(string claimNumber);
        Task<IEnumerable<string>> GetRequiredDocumentsAsync(string claimNumber);
        Task<IEnumerable<object>> GetTimelineAsync(string claimNumber);
        Task<bool> UpdateStatusAsync(string claimId, string status, string? notes = null);
        Task<bool> ApproveClaimAsync(string claimId, decimal approvedAmount);
    }
}
