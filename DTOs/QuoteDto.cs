using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class QuoteDto
    {
        public int QuoteId { get; set; }
        public string QuoteNumber { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? AgentId { get; set; }
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public PremiumFrequency PremiumFrequency { get; set; }
        public DateTime ValidUntil { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class QuoteCreateDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int? AgentId { get; set; }
        public decimal SumAssured { get; set; }
        public PremiumFrequency PremiumFrequency { get; set; }
    }
}
