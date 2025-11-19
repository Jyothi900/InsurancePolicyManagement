using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IPolicyRepository
    {
        Task<IEnumerable<Policy>> GetAllAsync();
        Task<Policy?> GetByIdAsync(string id);

        Task<IEnumerable<Policy>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Policy>> GetByStatusAsync(Status status);
        Task<Policy> AddAsync(Policy policy);
        Task<Policy> UpdateAsync(Policy policy);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string policyId, Status status);
        Task<bool> UpdateNextPremiumDueAsync(string policyId, DateTime nextDueDate);
        Task<Policy?> GetLastPolicyAsync();
        Task<Claim> AddClaimAsync(Claim claim);
        Task<PolicyProduct?> GetProductByIdAsync(string productId);
    }
}
