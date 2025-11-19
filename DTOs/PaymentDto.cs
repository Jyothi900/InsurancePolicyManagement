using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{


    public class PaymentInitiateDto
    {
        public string PolicyId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ReturnUrl { get; set; } = string.Empty;
    }

    public class PaymentStatusDto
    {
        public string PaymentId { get; set; } = string.Empty;
        public string PolicyId { get; set; } = string.Empty;
        public string? ProposalId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentType { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? TransactionId { get; set; }
        public string? PaymentGateway { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public string? FailureReason { get; set; }
    }
}
