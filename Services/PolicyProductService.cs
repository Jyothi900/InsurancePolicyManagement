using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;

namespace InsurancePolicyManagement.Services
{
    public class PolicyProductService : IPolicyProductService
    {
        private readonly IPolicyProductRepository _repo;
        private readonly IMapper _mapper;

        public PolicyProductService(IPolicyProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PolicyProductDto>> GetAllAsync()
        {
            var products = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<PolicyProductDto>>(products);
        }

        public async Task<PolicyProductDto?> GetByIdAsync(string id)
        {
            var product = await _repo.GetActiveByIdAsync(id);
            return product == null ? null : _mapper.Map<PolicyProductDto>(product);
        }

        public async Task<IEnumerable<PolicyProductDto>> GetByCategoryAsync(string category)
        {
            var products = await _repo.GetByCategoryAsync(category);
            return _mapper.Map<IEnumerable<PolicyProductDto>>(products);
        }

        public async Task<QuoteResponseDto?> CalculatePremiumAsync(QuoteRequestDto dto)
        {
            try
            {
                var product = await _repo.GetActiveByIdAsync(dto.ProductId);
                if (product == null) 
                {
                    Console.WriteLine($"Product not found: {dto.ProductId}");
                    return null;
                }

                // Log product constraints for debugging
                Console.WriteLine($"Product {dto.ProductId} constraints:");
                Console.WriteLine($"Age: {product.MinAge}-{product.MaxAge}, Requested: {dto.Age}");
                Console.WriteLine($"Sum Assured: {product.MinSumAssured}-{product.MaxSumAssured}, Requested: {dto.SumAssured}");
                Console.WriteLine($"Term: {product.MinTerm}-{product.MaxTerm}, Requested: {dto.TermYears}");

                if (dto.Age < product.MinAge || dto.Age > product.MaxAge) 
                {
                    Console.WriteLine($"Age validation failed for {dto.ProductId}");
                    return null;
                }
                if (dto.SumAssured < product.MinSumAssured || dto.SumAssured > product.MaxSumAssured) 
                {
                    Console.WriteLine($"Sum assured validation failed for {dto.ProductId}");
                    return null;
                }
                if (dto.TermYears < product.MinTerm || dto.TermYears > product.MaxTerm) 
                {
                    Console.WriteLine($"Term validation failed for {dto.ProductId}");
                    return null;
                }

            decimal basePremium = (dto.SumAssured / 1000) * product.BaseRate;

            
            if (dto.Age >= 18 && dto.Age <= 25) basePremium *= 0.8m;  
            else if (dto.Age >= 26 && dto.Age <= 35) basePremium *= 1.0m; 
            else if (dto.Age >= 36 && dto.Age <= 45) basePremium *= 1.2m;  
            else if (dto.Age >= 46 && dto.Age <= 55) basePremium *= 1.5m;  
            else if (dto.Age >= 56) basePremium *= 2.0m;  

           
            if (dto.IsSmoker) basePremium *= 1.75m;  
            if (dto.Gender == Gender.Female) basePremium *= 0.85m;  

            
            if (dto.SumAssured >= 5000000) basePremium *= 0.9m;  
            else if (dto.SumAssured >= 2000000) basePremium *= 0.95m;  

            
            if (dto.TermYears >= 30) basePremium *= 1.1m;  
            else if (dto.TermYears <= 10) basePremium *= 1.05m;  

                return new QuoteResponseDto
                {
                    ProductId = dto.ProductId,
                    PremiumAmount = Math.Round(basePremium, 2),
                    ProductName = product.ProductName,
                    SumAssured = dto.SumAssured,
                    PremiumFrequency = PremiumFrequency.Annual,
                    ValidUntil = DateTime.Now.AddDays(30)
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _repo.GetCategoriesAsync();
        }

        public async Task<IEnumerable<string>> GetInsuranceTypesAsync()
        {
            return await _repo.GetInsuranceTypesAsync();
        }

        public async Task<bool> CheckEligibilityAsync(string productId, int age)
        {
            var product = await _repo.GetActiveByIdAsync(productId);
            if (product == null) return false;

            return age >= product.MinAge && age <= product.MaxAge;
        }

        public async Task<PolicyProductDto> CreateAsync(PolicyProductCreateDto dto)
        {
            var product = _mapper.Map<PolicyProduct>(dto);
            var created = await _repo.AddAsync(product);
            return _mapper.Map<PolicyProductDto>(created);
        }

        public async Task<PolicyProductDto?> UpdateAsync(string id, PolicyProductUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

           
            // Only update fields that exist in the model and are explicitly provided
            if (!string.IsNullOrEmpty(dto.ProductName)) existing.ProductName = dto.ProductName;
            if (!string.IsNullOrEmpty(dto.CompanyName)) existing.CompanyName = dto.CompanyName;
            if (!string.IsNullOrEmpty(dto.Category)) existing.Category = Enum.Parse<PolicyType>(dto.Category);
            if (dto.InsuranceType.HasValue) existing.InsuranceType = dto.InsuranceType.Value;
            if (dto.MinAge.HasValue && dto.MinAge > 0) existing.MinAge = dto.MinAge.Value;
            if (dto.MaxAge.HasValue && dto.MaxAge > 0) existing.MaxAge = dto.MaxAge.Value;
            if (dto.MinSumAssured.HasValue && dto.MinSumAssured > 0) existing.MinSumAssured = dto.MinSumAssured.Value;
            if (dto.MaxSumAssured.HasValue && dto.MaxSumAssured > 0) existing.MaxSumAssured = dto.MaxSumAssured.Value;
            if (dto.MinTerm.HasValue && dto.MinTerm > 0) existing.MinTerm = dto.MinTerm.Value;
            if (dto.MaxTerm.HasValue && dto.MaxTerm > 0) existing.MaxTerm = dto.MaxTerm.Value;
            if (dto.BaseRate.HasValue && dto.BaseRate > 0) existing.BaseRate = dto.BaseRate.Value;
            if (dto.RiskLevel.HasValue) existing.RiskLevel = dto.RiskLevel.Value;
            if (dto.HasMaturityBenefit.HasValue) existing.HasMaturityBenefit = dto.HasMaturityBenefit.Value;
            if (dto.HasDeathBenefit.HasValue) existing.HasDeathBenefit = dto.HasDeathBenefit.Value;
            if (dto.IsActive.HasValue) existing.IsActive = dto.IsActive.Value;

            var updated = await _repo.UpdateAsync(existing);
            return _mapper.Map<PolicyProductDto>(updated);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repo.DeleteAsync(id);
        }
    }
}
