namespace InsurancePolicyManagement.DTOs
{
    public class PaymentByUserRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class PaymentByIdRequest
    {
        public string PaymentId { get; set; } = string.Empty;
    }

    public class PaymentReceiptRequest
    {
        public string TransactionId { get; set; } = string.Empty;
    }

    public class PayPremiumRequest
    {
        public string PolicyId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }

    public class PayProposalRequest
    {
        public string ProposalId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
    }
}