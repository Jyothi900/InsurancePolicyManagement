using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IClaimRepository
    {
        Task<IEnumerable<Claim>> GetAllAsync();
        Task<Claim?> GetByIdAsync(string id);
        Task<Claim?> GetByClaimNumberAsync(string claimNumber);
        Task<IEnumerable<Claim>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Claim>> GetByPolicyIdAsync(string policyId);
        Task<IEnumerable<Claim>> GetByStatusAsync(string status);
        Task<Claim> AddAsync(Claim claim);
        Task<Claim> UpdateAsync(Claim claim);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string claimId, string status, string? notes = null);
        Task<bool> ApproveClaimAsync(string claimId, decimal approvedAmount);
    }
}
