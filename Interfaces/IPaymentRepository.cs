using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(string id);
        Task<Payment?> GetByTransactionIdAsync(string transactionId);
        Task<IEnumerable<Payment>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Payment>> GetByPolicyIdAsync(string policyId);
        Task<Payment?> GetByProposalIdAsync(string proposalId);
        Task<IEnumerable<Payment>> GetByStatusAsync(string status);
        Task<Payment> AddAsync(Payment payment);
        Task<Payment> UpdateAsync(Payment payment);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateStatusAsync(string paymentId, string status, DateTime? processedDate = null);
    }
}
