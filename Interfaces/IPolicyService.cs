using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IPolicyService
    {
        Task<IEnumerable<PolicyDTO>> GetByUserIdAsync(string userId);
        Task<PolicyDTO?> GetByIdAsync(string id);
        Task<IEnumerable<PolicyDTO>> GetAllAsync();

        Task<object?> GetPremiumScheduleAsync(string policyId);
        Task<bool> SurrenderAsync(string policyId, string userId);
        Task<bool> ReviveAsync(string policyId, string userId);
        Task<PolicyDTO?> IssueFromProposalAsync(string proposalId);
        Task<byte[]?> GeneratePolicyPDFAsync(string policyId);
    }
}
