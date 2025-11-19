using System.ComponentModel.DataAnnotations;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Models
{
    public class Policy
    {
        [Key]
        public string PolicyId { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 10)]
        [RegularExpression(@"^[A-Z0-9/\-]+$")]
        public string? PolicyNumber { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string ProductId { get; set; } = string.Empty;

        public string? AgentId { get; set; } 

        [Required]
        public PolicyType PolicyType { get; set; } 

        [Required]
        [Range(50000, 1000000000)]
        public decimal SumAssured { get; set; }

        [Required]
        [Range(1000, 10000000)]
        public decimal PremiumAmount { get; set; }

        [Required]
        public PremiumFrequency PremiumFrequency { get; set; } = PremiumFrequency.Annual;

        [Range(1, 100)]
        public int TermYears { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime? ExpiryDate { get; set; }
        public DateTime? NextPremiumDue { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Active;

        public DateTime IssuedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public User? User { get; set; }
        public PolicyProduct? Product { get; set; }
        public User? Agent { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Claim>? Claims { get; set; }
        public ICollection<Document>? Documents { get; set; }
        public ICollection<Nominee>? Nominees { get; set; }
    }
}
