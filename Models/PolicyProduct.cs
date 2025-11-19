using System.ComponentModel.DataAnnotations;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Models
{
    public class PolicyProduct
    {
        [Key]
        public string ProductId { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 5)]
        public string? ProductName { get; set; }

        [Required]
        public PolicyType Category { get; set; }

        [Required]
        public InsuranceType InsuranceType { get; set; } = InsuranceType.Life;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string? CompanyName { get; set; } 

        [Range(0, 80)]
        public int MinAge { get; set; } = 18;
        
        [Range(18, 100)]
        public int MaxAge { get; set; } = 65;

        [Range(50000, 10000000)]
        public decimal MinSumAssured { get; set; } = 100000;
        
        [Range(100000, 10000000)]
        public decimal MaxSumAssured { get; set; } = 10000000;

        [Range(1, 50)]
        public int MinTerm { get; set; } = 5;
        
        [Range(5, 100)]
        public int MaxTerm { get; set; } = 30;

        [Range(0.1, 1000)]
        public decimal BaseRate { get; set; }

        public bool HasMaturityBenefit { get; set; } = false;
        public bool HasDeathBenefit { get; set; } = true;

        public RiskLevel RiskLevel { get; set; } = RiskLevel.Low;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        
        public ICollection<Policy>? Policies { get; set; }
        public ICollection<Proposal>? Proposals { get; set; }
    }
}
