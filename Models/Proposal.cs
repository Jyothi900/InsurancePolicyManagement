using System.ComponentModel.DataAnnotations;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Models
{
    public class Proposal
    {
        [Key]
        public string ProposalId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string ProductId { get; set; } = string.Empty;

        public string? AgentId { get; set; } // Agent who created proposal

        [Required]
        [Range(50000, 10000000)]
        public decimal SumAssured { get; set; }

        [Required]
        [Range(1, 100)]
        public int TermYears { get; set; }

        [Required]
        [Range(1000, 10000000)]
        public decimal PremiumAmount { get; set; }

        [Required]
        public PremiumFrequency PremiumFrequency { get; set; } = PremiumFrequency.Annual;

        
        [Range(50, 250)]
        public decimal Height { get; set; }
        
        [Range(20, 300)]
        public decimal Weight { get; set; }
        public bool IsSmoker { get; set; } = false;
        public bool IsDrinker { get; set; } = false;
        public string? PreExistingConditions { get; set; }

        // Employment
        [StringLength(100)]
        public string? Occupation { get; set; }
        
        [Range(100000, 10000000)]
        public decimal AnnualIncome { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Applied;

        public string? UnderwritingNotes { get; set; }

        public DateTime AppliedDate { get; set; } = DateTime.Now;
        public DateTime? ReviewedDate { get; set; }

        // Navigation Properties
        public User? User { get; set; }
        public PolicyProduct? Product { get; set; }
        public User? Agent { get; set; }
        public ICollection<Document>? Documents { get; set; }
        public ICollection<Nominee>? Nominees { get; set; }
    }
}
