using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Services;
using InsurancePolicyManagement.Extensions;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Repository
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly InsuranceManagementContext _context;

        public PolicyRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Policy>> GetAllAsync()
        {
            return await _context.Policies
                .WithStandardIncludes()
                .ToListAsync();
        }

        public async Task<Policy?> GetByIdAsync(string id)
        {
            return await _context.Policies
                .WithAllIncludes()
                .FirstOrDefaultAsync(p => p.PolicyId == id);
        }



        public async Task<IEnumerable<Policy>> GetByUserIdAsync(string userId)
        {
            return await _context.Policies
                .WithStandardIncludes()
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.IssuedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Policy>> GetByStatusAsync(Status status)
        {
            return await _context.Policies
                .WithStandardIncludes()
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<Policy> AddAsync(Policy policy)
        {
            try
            {
                var lastPolicy = await _context.Policies
                                         .OrderByDescending(p => p.PolicyId)
                                         .FirstOrDefaultAsync();

                string newId = lastPolicy == null
                    ? "POL001"
                    : "POL" + (int.Parse(lastPolicy.PolicyId.Substring(3)) + 1).ToString("D3");

                policy.PolicyId = newId;
                _context.Policies.Add(policy);
                await _context.SaveChangesAsync();
                return policy;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to create policy");
            }
        }

        public async Task<Policy> UpdateAsync(Policy policy)
        {
            _context.Policies.Update(policy);
            await _context.SaveChangesAsync();
            return policy;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == id);
            if (existing == null) return false;

            _context.Policies.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(string policyId, Status status)
        {
            try
            {
                var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == policyId);
                if (policy == null) return false;

                policy.Status = status;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateNextPremiumDueAsync(string policyId, DateTime nextDueDate)
        {
            var policy = await _context.Policies.FirstOrDefaultAsync(p => p.PolicyId == policyId);
            if (policy == null) return false;

            policy.NextPremiumDue = nextDueDate;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Policy?> GetLastPolicyAsync()
        {
            return await _context.Policies
                .OrderByDescending(p => p.PolicyId)
                .FirstOrDefaultAsync();
        }

        public async Task<Claim> AddClaimAsync(Claim claim)
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
        
        public async Task<PolicyProduct?> GetProductByIdAsync(string productId)
        {
            return await _context.PolicyProducts
                .FirstOrDefaultAsync(p => p.ProductId == productId);
        }
    }
}
