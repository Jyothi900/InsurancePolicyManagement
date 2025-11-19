using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Repository
{
    public class NomineeRepository : INomineeRepository
    {
        private readonly InsuranceManagementContext _context;

        public NomineeRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Nominee>> GetAllAsync()
        {
            return await _context.Nominees
                .Include(n => n.Policy)
                .Include(n => n.Proposal)
                .ToListAsync();
        }

        public async Task<Nominee?> GetByIdAsync(string id)
        {
            return await _context.Nominees
                .Include(n => n.Policy)
                .Include(n => n.Proposal)
                .FirstOrDefaultAsync(n => n.NomineeId == id);
        }

        public async Task<IEnumerable<Nominee>> GetByPolicyIdAsync(string policyId)
        {
            return await _context.Nominees
                .Where(n => n.PolicyId == policyId)
                .OrderBy(n => n.SharePercentage)
                .ToListAsync();
        }

        public async Task<IEnumerable<Nominee>> GetByProposalIdAsync(string proposalId)
        {
            return await _context.Nominees
                .Where(n => n.ProposalId == proposalId)
                .OrderBy(n => n.SharePercentage)
                .ToListAsync();
        }

        public async Task<Nominee> AddAsync(Nominee nominee)
        {
            var lastNominee = await _context.Nominees
                                     .OrderByDescending(n => n.NomineeId)
                                     .FirstOrDefaultAsync();

            string newId = lastNominee == null
                ? "NOM001"
                : "NOM" + (int.Parse(lastNominee.NomineeId.Substring(3)) + 1).ToString("D3");

            nominee.NomineeId = newId;
            _context.Nominees.Add(nominee);
            await _context.SaveChangesAsync();
            return nominee;
        }

        public async Task<Nominee> UpdateAsync(Nominee nominee)
        {
            _context.Nominees.Update(nominee);
            await _context.SaveChangesAsync();
            return nominee;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.Nominees.FirstOrDefaultAsync(n => n.NomineeId == id);
            if (existing == null) return false;

            _context.Nominees.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByPolicyIdAsync(string policyId)
        {
            var nominees = await _context.Nominees.Where(n => n.PolicyId == policyId).ToListAsync();
            if (!nominees.Any()) return false;

            _context.Nominees.RemoveRange(nominees);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ValidateSharePercentageAsync(string? policyId, string? proposalId, string? excludeNomineeId = null)
        {
            var query = _context.Nominees.AsQueryable();

            if (!string.IsNullOrEmpty(policyId))
                query = query.Where(n => n.PolicyId == policyId);
            else if (!string.IsNullOrEmpty(proposalId))
                query = query.Where(n => n.ProposalId == proposalId);
            else
                return false;

            if (!string.IsNullOrEmpty(excludeNomineeId))
                query = query.Where(n => n.NomineeId != excludeNomineeId);

            var totalShare = await query.SumAsync(n => n.SharePercentage);
            return totalShare <= 100;
        }
    }
}
