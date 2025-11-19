using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IPolicyProductService
    {
        Task<IEnumerable<PolicyProductDto>> GetAllAsync();
        Task<PolicyProductDto?> GetByIdAsync(string id);
        Task<IEnumerable<PolicyProductDto>> GetByCategoryAsync(string category);
        Task<QuoteResponseDto?> CalculatePremiumAsync(QuoteRequestDto dto);
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<IEnumerable<string>> GetInsuranceTypesAsync();
        Task<bool> CheckEligibilityAsync(string productId, int age);
        Task<PolicyProductDto> CreateAsync(PolicyProductCreateDto dto);
        Task<PolicyProductDto?> UpdateAsync(string id, PolicyProductUpdateDto dto);
        Task<bool> DeleteAsync(string id);
    }
}
