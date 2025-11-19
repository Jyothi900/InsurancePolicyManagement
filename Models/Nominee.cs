using System.ComponentModel.DataAnnotations;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Models
{
    public class Nominee
    {
        [Key]
        public string NomineeId { get; set; } = string.Empty;

        public string? ProposalId { get; set; }
        public string? PolicyId { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s.]+$")]
        public string? FullName { get; set; }

        [Required]
        public UnifiedRelationship Relationship { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [Range(0.01, 100)]
        public decimal SharePercentage { get; set; } = 100;

        [StringLength(12, MinimumLength = 12)]
        [RegularExpression(@"^[0-9]{12}$")]
        public string? AadhaarNumber { get; set; }

        [StringLength(500, MinimumLength = 10)]
        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Proposal? Proposal { get; set; }
        public Policy? Policy { get; set; }
    }
}
