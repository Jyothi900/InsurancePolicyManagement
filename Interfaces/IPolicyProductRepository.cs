using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IPolicyProductRepository
    {
        Task<IEnumerable<PolicyProduct>> GetAllAsync();
        Task<PolicyProduct?> GetByIdAsync(string id);
        Task<PolicyProduct?> GetActiveByIdAsync(string id);
        Task<IEnumerable<PolicyProduct>> GetByCategoryAsync(string category);
        Task<IEnumerable<PolicyProduct>> GetByCompanyAsync(string companyName);
        Task<PolicyProduct> AddAsync(PolicyProduct product);
        Task<PolicyProduct> UpdateAsync(PolicyProduct product);
        Task<bool> DeleteAsync(string id);
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<IEnumerable<string>> GetInsuranceTypesAsync();
    }
}
