using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class PolicyProductDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public PolicyType PolicyType { get; set; }
        public decimal MinSumAssured { get; set; }
        public decimal MaxSumAssured { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int PolicyTerm { get; set; }
        public decimal PremiumRate { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Category { get; set; } = string.Empty;
        public InsuranceType InsuranceType { get; set; }
        public RiskLevel RiskLevel { get; set; }
    }

    public class PolicyProductCreateDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty; 
        public PolicyType PolicyType { get; set; }
        public decimal MinSumAssured { get; set; }
        public decimal MaxSumAssured { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int PolicyTerm { get; set; }
        public decimal PremiumRate { get; set; }
        public string Category { get; set; } = string.Empty;
        public InsuranceType InsuranceType { get; set; }
        public RiskLevel RiskLevel { get; set; }
    }


    public class PolicyProductUpdateDto
    {
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public decimal? MinSumAssured { get; set; }
        public decimal? MaxSumAssured { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? PolicyTerm { get; set; }
        public decimal? PremiumRate { get; set; }
        public bool? IsActive { get; set; }
        public string? Category { get; set; }
        public InsuranceType? InsuranceType { get; set; }
        public string? CompanyName { get; set; }
        public int? MinTerm { get; set; }
        public int? MaxTerm { get; set; }
        public decimal? BaseRate { get; set; }
        public bool? HasMaturityBenefit { get; set; }
        public bool? HasDeathBenefit { get; set; }
        public RiskLevel? RiskLevel { get; set; }
    }

    public class QuoteRequestDto
    {
        public string ProductId { get; set; } = string.Empty;
        public decimal SumAssured { get; set; }
        public int Age { get; set; }
        public PremiumFrequency PremiumFrequency { get; set; }
        public int TermYears { get; set; }
        public bool IsSmoker { get; set; }
        public Gender Gender { get; set; }
    }

    public class QuoteResponseDto
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public decimal SumAssured { get; set; }
        public decimal PremiumAmount { get; set; }
        public PremiumFrequency PremiumFrequency { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
