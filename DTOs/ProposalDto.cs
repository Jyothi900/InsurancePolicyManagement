using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class ProposalDto
    {
        public string ProposalId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public PremiumFrequency PremiumFrequency { get; set; }
        public Status Status { get; set; }
        public DateTime AppliedDate { get; set; }
        public DateTime? ReviewedDate { get; set; }
        public bool IsSmoker { get; set; }
        public int TermYears { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public bool IsDrinker { get; set; }
        public string PreExistingConditions { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public decimal AnnualIncome { get; set; }
    }

    public class ProposalCreateDto
    {
        public string ProductId { get; set; } = string.Empty;
        public decimal SumAssured { get; set; }
        public PremiumFrequency PremiumFrequency { get; set; }
        public bool IsSmoker { get; set; }
        public int TermYears { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public bool IsDrinker { get; set; }
        public string PreExistingConditions { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public decimal AnnualIncome { get; set; }
    }
}
