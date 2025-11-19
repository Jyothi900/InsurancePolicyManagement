using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class PolicyDTO
    {
        public string PolicyId { get; set; } = string.Empty;
        public string PolicyNumber { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string? AgentId { get; set; }
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public PremiumFrequency PremiumFrequency { get; set; }
        public int TermYears { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public Status Status { get; set; }
        public DateTime IssuedDate { get; set; }
    }
}
