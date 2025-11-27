using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface ICustomerJourneyService
    {
        Task<object> GetCustomerJourneyDataAsync(string? userId, string? productId, QuoteRequestDto? quoteRequest);
        Task<object> CalculatePremiumWithContextAsync(string? userId, QuoteRequestDto quoteRequest);
    }
}