using AutoMapper;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Services
{
    public class QuoteService : IQuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IMapper _mapper;

        public QuoteService(IQuoteRepository quoteRepository, IMapper mapper)
        {
            _quoteRepository = quoteRepository;
            _mapper = mapper;
        }

        public async Task<QuoteDto> CreateQuoteAsync(QuoteCreateDto quoteDto)
        {
            var quote = _mapper.Map<Quote>(quoteDto);
            quote.PremiumAmount = CalculatePremium(quote.SumAssured, quote.TermYears);
            
            var createdQuote = await _quoteRepository.CreateAsync(quote);
            return _mapper.Map<QuoteDto>(createdQuote);
        }

        public async Task<QuoteDto?> GetQuoteByIdAsync(string quoteId)
        {
            var quote = await _quoteRepository.GetByIdAsync(quoteId);
            return quote == null ? null : _mapper.Map<QuoteDto>(quote);
        }

        public async Task<IEnumerable<QuoteDto>> GetQuotesByUserIdAsync(string userId)
        {
            var quotes = await _quoteRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<QuoteDto>>(quotes);
        }

        public async Task<IEnumerable<QuoteDto>> GetQuotesByAgentIdAsync(string agentId)
        {
            var quotes = await _quoteRepository.GetByAgentIdAsync(agentId);
            return _mapper.Map<IEnumerable<QuoteDto>>(quotes);
        }

        public async Task<bool> ConvertToProposalAsync(string quoteId)
        {
            var quote = await _quoteRepository.GetByIdAsync(quoteId);
            if (quote == null) return false;

            quote.IsConverted = true;
            await _quoteRepository.UpdateAsync(quote);
            return true;
        }

        private decimal CalculatePremium(decimal sumAssured, int termYears)
        {
          
            return (sumAssured * 0.02m * termYears) / termYears;
        }
    }
}
