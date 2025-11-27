namespace InsurancePolicyManagement.DTOs
{
    public class CustomerJourneyRequest
    {
        public string? UserId { get; set; }
        public string? ProductId { get; set; }
        public QuoteRequestDto? QuoteRequest { get; set; }
    }

    public class PremiumCalculationRequest
    {
        public string? UserId { get; set; }
        public QuoteRequestDto QuoteRequest { get; set; } = new();
    }
}