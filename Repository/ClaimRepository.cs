using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Extensions;

namespace InsurancePolicyManagement.Repository
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly InsuranceManagementContext _context;

        public ClaimRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Claim>> GetAllAsync()
        {
            return await _context.Claims
                .WithStandardIncludes()
                .ToListAsync();
        }

        public async Task<Claim?> GetByIdAsync(string id)
        {
            return await _context.Claims
                .WithStandardIncludes()
                .FirstOrDefaultAsync(c => c.ClaimId == id);
        }

        public async Task<Claim?> GetByClaimNumberAsync(string claimNumber)
        {
            return await _context.Claims
                .WithStandardIncludes()
                .FirstOrDefaultAsync(c => c.ClaimNumber == claimNumber);
        }

        public async Task<IEnumerable<Claim>> GetByUserIdAsync(string userId)
        {
            return await _context.Claims
                .Include(c => c.Policy)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.FiledDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Claim>> GetByPolicyIdAsync(string policyId)
        {
            return await _context.Claims
                .Include(c => c.User)
                .Where(c => c.PolicyId == policyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Claim>> GetByStatusAsync(string status)
        {
            return await _context.Claims
                .WithStandardIncludes()
                .Where(c => c.Status == status)
                .ToListAsync();
        }

        public async Task<Claim> AddAsync(Claim claim)
        {
            var lastClaim = await _context.Claims
                                     .OrderByDescending(c => c.ClaimId)
                                     .FirstOrDefaultAsync();

            string newId = lastClaim == null
                ? "CLM001"
                : "CLM" + (int.Parse(lastClaim.ClaimId.Substring(3)) + 1).ToString("D3");

            claim.ClaimId = newId;
            _context.Claims.Add(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        public async Task<Claim> UpdateAsync(Claim claim)
        {
            _context.Claims.Update(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == id);
            if (existing == null) return false;

            _context.Claims.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(string claimId, string status, string? notes = null)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == claimId);
            if (claim == null) return false;

            claim.Status = status;
            if (!string.IsNullOrEmpty(notes))
                claim.InvestigationNotes = notes;
            
            if (status == "Approved" || status == "Rejected")
                claim.ProcessedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ApproveClaimAsync(string claimId, decimal approvedAmount)
        {
            var claim = await _context.Claims.FirstOrDefaultAsync(c => c.ClaimId == claimId);
            if (claim == null) return false;

            claim.Status = "Approved";
            claim.ApprovedAmount = approvedAmount;
            claim.ProcessedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
