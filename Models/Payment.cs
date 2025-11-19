using System.ComponentModel.DataAnnotations;

namespace InsurancePolicyManagement.Models
{
    public class Payment
    {
        [Key]
        public string PaymentId { get; set; } = string.Empty;

        public string? PolicyId { get; set; }

        public string? ProposalId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [Range(100, 10000000)]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentType { get; set; } = "Premium";

        [Required]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(50)]
        public string? TransactionId { get; set; }

        [StringLength(30)]
        public string? PaymentGateway { get; set; }

        [Required]
        public string Status { get; set; } = "Pending";

        public DateTime PaymentDate { get; set; } = DateTime.Now;
        public DateTime? ProcessedDate { get; set; }

        public string? FailureReason { get; set; }

        // For Premium Payments
        public DateTime? DueDate { get; set; }
        public DateTime? NextDueDate { get; set; }

        // Navigation Properties
        public Policy? Policy { get; set; }
        public User? User { get; set; }
    }
}
