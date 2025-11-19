using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IQuoteService
    {
        Task<QuoteDto> CreateQuoteAsync(QuoteCreateDto quoteDto);
        Task<QuoteDto?> GetQuoteByIdAsync(string quoteId);
        Task<IEnumerable<QuoteDto>> GetQuotesByUserIdAsync(string userId);
        Task<IEnumerable<QuoteDto>> GetQuotesByAgentIdAsync(string agentId);
        Task<bool> ConvertToProposalAsync(string quoteId);
    }
}
