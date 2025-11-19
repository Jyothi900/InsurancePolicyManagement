using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IProposalRepository
    {
        Task<IEnumerable<Proposal>> GetAllAsync();
        Task<Proposal?> GetByIdAsync(string id);
        Task<IEnumerable<Proposal>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Proposal>> GetByStatusAsync(Status status);
        Task<Proposal> AddAsync(Proposal proposal);
        Task<Proposal> UpdateAsync(Proposal proposal);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string proposalId, Status status, string? notes = null);
    }
}
