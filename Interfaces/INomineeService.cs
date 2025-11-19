using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface INomineeService
    {
        Task<IEnumerable<NomineeDto>> GetByPolicyIdAsync(string policyId);
        Task<IEnumerable<NomineeDto>> GetByProposalIdAsync(string proposalId);
        Task<NomineeDto?> AddAsync(NomineeDto dto, string? policyId = null, string? proposalId = null);
        Task<NomineeDto?> UpdateAsync(string id, NomineeDto dto);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAllForPolicyAsync(string policyId, List<NomineeDto> nominees);
        Task<NomineeDto?> GetByIdAsync(string id);
    }
}
