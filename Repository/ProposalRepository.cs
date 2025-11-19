using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Repository
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly InsuranceManagementContext _context;

        public ProposalRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proposal>> GetAllAsync()
        {
            return await _context.Proposals
                .Include(p => p.User)
                .Include(p => p.Product)
                .ToListAsync();
        }

        public async Task<Proposal?> GetByIdAsync(string id)
        {
            return await _context.Proposals
                .Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Nominees)
                .FirstOrDefaultAsync(p => p.ProposalId == id);
        }

        public async Task<IEnumerable<Proposal>> GetByUserIdAsync(string userId)
        {
            return await _context.Proposals
                .Include(p => p.Product)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.AppliedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Proposal>> GetByStatusAsync(Status status)
        {
            return await _context.Proposals
                .Include(p => p.User)
                .Include(p => p.Product)
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<Proposal> AddAsync(Proposal proposal)
        {
            // Generate ProposalId in repository (data layer)
            proposal.ProposalId = GenerateProposalId(proposal.UserId);
            _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();
            return proposal;
        }

        private static string GenerateProposalId(string userId)
        {
            var year = DateTime.Now.Year;
            var last3Digits = userId.Length >= 3 ? userId.Substring(userId.Length - 3) : userId.PadLeft(3, '0');
            return $"PROP/{year}/{last3Digits}";
        }

        public async Task<Proposal> UpdateAsync(Proposal proposal)
        {
            _context.Proposals.Update(proposal);
            await _context.SaveChangesAsync();
            return proposal;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.Proposals.FirstOrDefaultAsync(p => p.ProposalId == id);
            if (existing == null) return false;

            _context.Proposals.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(string proposalId, Status status, string? notes = null)
        {
            var proposal = await _context.Proposals.FirstOrDefaultAsync(p => p.ProposalId == proposalId);
            if (proposal == null) return false;

            proposal.Status = status;
            if (!string.IsNullOrEmpty(notes))
                proposal.UnderwritingNotes = notes;
            
            if (status == Status.Approved || status == Status.Rejected)
                proposal.ReviewedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
