using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Extensions;

namespace InsurancePolicyManagement.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly InsuranceManagementContext _context;

        public PaymentRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetAllAsync()
        {
            return await _context.Payments
                .WithStandardIncludes()
                .ToListAsync();
        }

        public async Task<Payment?> GetByIdAsync(string id)
        {
            return await _context.Payments
                .WithStandardIncludes()
                .FirstOrDefaultAsync(p => p.PaymentId == id);
        }

        public async Task<Payment?> GetByTransactionIdAsync(string transactionId)
        {
            return await _context.Payments
                .WithStandardIncludes()
                .FirstOrDefaultAsync(p => p.TransactionId == transactionId);
        }

        public async Task<IEnumerable<Payment>> GetByUserIdAsync(string userId)
        {
            return await _context.Payments
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .Take(50) 
                .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetByPolicyIdAsync(string policyId)
        {
            return await _context.Payments
                .Where(p => p.PolicyId == policyId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();
        }

        public async Task<Payment?> GetByProposalIdAsync(string proposalId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.ProposalId == proposalId);
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(string status)
        {
            return await _context.Payments
                .WithStandardIncludes()
                .Where(p => p.Status == status)
                .ToListAsync();
        }

        public async Task<Payment> AddAsync(Payment payment)
        {
            try
            {
                var lastPayment = await _context.Payments
                                         .OrderByDescending(p => p.PaymentId)
                                         .FirstOrDefaultAsync();

                string newId = lastPayment == null
                    ? "PAY001"
                    : "PAY" + (int.Parse(lastPayment.PaymentId.Substring(3)) + 1).ToString("D3");

                payment.PaymentId = newId;
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
                return payment;
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to create payment");
            }
        }

        public async Task<Payment> UpdateAsync(Payment payment)
        {
            _context.Payments.Update(payment);
            await _context.SaveChangesAsync();
            return payment;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == id);
            if (existing == null) return false;

            _context.Payments.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateStatusAsync(string paymentId, string status, DateTime? processedDate = null)
        {
            try
            {
                var payment = await _context.Payments.FirstOrDefaultAsync(p => p.PaymentId == paymentId);
                if (payment == null) return false;

                payment.Status = status;
                if (processedDate.HasValue)
                    payment.ProcessedDate = processedDate.Value;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
