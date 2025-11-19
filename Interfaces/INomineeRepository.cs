using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface INomineeRepository
    {
        Task<IEnumerable<Nominee>> GetAllAsync();
        Task<Nominee?> GetByIdAsync(string id);
        Task<IEnumerable<Nominee>> GetByPolicyIdAsync(string policyId);
        Task<IEnumerable<Nominee>> GetByProposalIdAsync(string proposalId);
        Task<Nominee> AddAsync(Nominee nominee);
        Task<Nominee> UpdateAsync(Nominee nominee);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteByPolicyIdAsync(string policyId);
        Task<bool> ValidateSharePercentageAsync(string? policyId, string? proposalId, string? excludeNomineeId = null);
    }
}
