using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IQuoteRepository
    {
        Task<Quote> CreateAsync(Quote quote);
        Task<Quote?> GetByIdAsync(string quoteId);
        Task<IEnumerable<Quote>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Quote>> GetByAgentIdAsync(string agentId);
        Task<Quote> UpdateAsync(Quote quote);
        Task<bool> DeleteAsync(string quoteId);
    }
}
