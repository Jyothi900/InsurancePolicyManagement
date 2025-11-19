using System.ComponentModel.DataAnnotations;

namespace InsurancePolicyManagement.Models
{
    public class Claim
    {
        [Key]
        public string ClaimId { get; set; } = string.Empty;

        [Required]
        [StringLength(20, MinimumLength = 8)]
        [RegularExpression(@"^CLM[0-9/\-A-Z]+$")]
        public string? ClaimNumber { get; set; }

        [Required]
        public string PolicyId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty; // Filed by (nominee/legal heir)

        public string? AgentId { get; set; } // Agent who submitted claim
        public string? UnderwriterId { get; set; } // Underwriter who validated claim

        [Required]
        public string ClaimType { get; set; } = "Death";

        [Required]
        [Range(1000, 10000000)]
        public decimal ClaimAmount { get; set; }

        [Range(0, 10000000)]
        public decimal? ApprovedAmount { get; set; }

        [Required]
        public string Status { get; set; } = "Filed";

        public DateTime IncidentDate { get; set; } // Date of death/maturity

        public DateTime FiledDate { get; set; } = DateTime.Now;

        public DateTime? ProcessedDate { get; set; }

        public string? CauseOfDeath { get; set; }

        [StringLength(100, MinimumLength = 2)]
        public string? ClaimantName { get; set; }
        
        public string? ClaimantRelation { get; set; }
        public string? ClaimantBankDetails { get; set; }

        public string? InvestigationNotes { get; set; }
        public string? RejectionReason { get; set; }

        public bool RequiresInvestigation { get; set; } = false;

        // Navigation Properties
        public Policy? Policy { get; set; }
        public User? User { get; set; }
        public User? Agent { get; set; }
        public User? Underwriter { get; set; }
        public ICollection<Document>? Documents { get; set; }
    }
}
