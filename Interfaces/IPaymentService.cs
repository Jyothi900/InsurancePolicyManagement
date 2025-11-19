using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IPaymentService
    {
        Task<IEnumerable<PaymentStatusDto>> GetHistoryAsync(string userId);
        Task<IEnumerable<object>> GetDuePremiumsAsync(string userId);
        Task<IEnumerable<object>> GetPendingPaymentsAsync(string userId);
        Task<IEnumerable<object>> GetIssuedProposalsAsync(string userId);
        Task<PaymentInitiateDto?> PayPremiumAsync(string policyId, string userId, string paymentMethod);
        Task<PaymentStatusDto?> PayProposalAsync(string proposalId, string userId, string paymentMethod);
        Task<PaymentStatusDto?> GetReceiptAsync(string transactionId);
        Task<PaymentStatusDto?> GetByIdAsync(string id);
    }
}
