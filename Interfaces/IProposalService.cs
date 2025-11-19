using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IProposalService
    {
        Task<ProposalDto?> SubmitAsync(string userId, ProposalCreateDto dto);
        Task<IEnumerable<ProposalDto>> GetAllAsync();
        Task<IEnumerable<ProposalDto>> GetByUserIdAsync(string userId);
        Task<ProposalDto?> GetByIdAsync(string id);
        Task<object?> GetStatusAsync(string id);
        Task<IEnumerable<string>> GetRequiredDocumentsAsync(string id);
        Task<bool> UpdateStatusAsync(string proposalId, string status, string? notes = null);
    }
}
