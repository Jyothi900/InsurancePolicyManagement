using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyManagement.Repository
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly InsuranceManagementContext _context;

        public QuoteRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<Quote> CreateAsync(Quote quote)
        {
            quote.QuoteId = Guid.NewGuid().ToString();
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
            return quote;
        }

        public async Task<Quote?> GetByIdAsync(string quoteId)
        {
            return await _context.Quotes
                .Include(q => q.User)
                .Include(q => q.Product)
                .Include(q => q.Agent)
                .FirstOrDefaultAsync(q => q.QuoteId == quoteId);
        }

        public async Task<IEnumerable<Quote>> GetByUserIdAsync(string userId)
        {
            return await _context.Quotes
                .Where(q => q.UserId == userId)
                .Include(q => q.Product)
                .ToListAsync();
        }

        public async Task<IEnumerable<Quote>> GetByAgentIdAsync(string agentId)
        {
            return await _context.Quotes
                .Where(q => q.AgentId == agentId)
                .Include(q => q.User)
                .Include(q => q.Product)
                .ToListAsync();
        }

        public async Task<Quote> UpdateAsync(Quote quote)
        {
            _context.Quotes.Update(quote);
            await _context.SaveChangesAsync();
            return quote;
        }

        public async Task<bool> DeleteAsync(string quoteId)
        {
            var quote = await _context.Quotes.FindAsync(quoteId);
            if (quote == null) return false;
            
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
