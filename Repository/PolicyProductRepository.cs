using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Repository
{
    public class PolicyProductRepository : IPolicyProductRepository
    {
        private readonly InsuranceManagementContext _context;

        public PolicyProductRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PolicyProduct>> GetAllAsync()
        {
            return await _context.PolicyProducts
                .Where(p => p.IsActive)
                .Take(100) 
                .ToListAsync();
        }

        public async Task<PolicyProduct?> GetByIdAsync(string id)
        {
            return await _context.PolicyProducts
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<PolicyProduct?> GetActiveByIdAsync(string id)
        {
            return await _context.PolicyProducts
                .FirstOrDefaultAsync(p => p.ProductId == id && p.IsActive);
        }

        public async Task<IEnumerable<PolicyProduct>> GetByCategoryAsync(string category)
        {
            return await _context.PolicyProducts
                .Where(p => p.InsuranceType.ToString() == category && p.IsActive)
                .Take(50) 
                .ToListAsync();
        }

        public async Task<IEnumerable<PolicyProduct>> GetByCompanyAsync(string companyName)
        {
            return await _context.PolicyProducts
                .Where(p => p.CompanyName == companyName && p.IsActive)
                .Take(50) 
                .ToListAsync();
        }

        public async Task<PolicyProduct> AddAsync(PolicyProduct product)
        {
            try
            {
                var lastProduct = await _context.PolicyProducts
                                         .OrderByDescending(p => p.ProductId)
                                         .FirstOrDefaultAsync();

                string newId = lastProduct == null
                    ? "PRD001"
                    : "PRD" + (int.Parse(lastProduct.ProductId.Substring(3)) + 1).ToString("D3");

                product.ProductId = newId;
                _context.PolicyProducts.Add(product);
                await _context.SaveChangesAsync();
                return product;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to create product");
            }
        }

        public async Task<PolicyProduct> UpdateAsync(PolicyProduct product)
        {
            _context.PolicyProducts.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var existing = await _context.PolicyProducts.FirstOrDefaultAsync(p => p.ProductId == id);
                if (existing == null) return false;

                existing.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.PolicyProducts
                .Where(p => p.IsActive)
                .Select(p => p.Category.ToString())
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetInsuranceTypesAsync()
        {
            return await _context.PolicyProducts
                .Where(p => p.IsActive)
                .Select(p => p.InsuranceType.ToString())
                .Distinct()
                .ToListAsync();
        }
    }
}
