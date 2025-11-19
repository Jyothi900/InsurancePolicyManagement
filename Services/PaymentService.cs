using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;

namespace InsurancePolicyManagement.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repo;
        private readonly IPolicyRepository _policyRepo;
        private readonly IProposalRepository _proposalRepo;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository repo, IPolicyRepository policyRepo, IProposalRepository proposalRepo, IMapper mapper)
        {
            _repo = repo;
            _policyRepo = policyRepo;
            _proposalRepo = proposalRepo;
            _mapper = mapper;
        }


        public async Task<IEnumerable<PaymentStatusDto>> GetHistoryAsync(string userId)
        {
            try
            {
                Console.WriteLine($"PaymentService.GetHistoryAsync called with userId: {userId}");
                
                Console.WriteLine("Calling repository.GetByUserIdAsync...");
                var payments = await _repo.GetByUserIdAsync(userId);
                
                Console.WriteLine($"Repository returned {payments?.Count() ?? 0} payments");
                
                Console.WriteLine("Mapping payments to DTOs...");
                var result = _mapper.Map<IEnumerable<PaymentStatusDto>>(payments);
                
                Console.WriteLine($"Mapping completed, returning {result?.Count() ?? 0} DTOs");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PaymentService.GetHistoryAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetDuePremiumsAsync(string userId)
        {
            var policies = await _policyRepo.GetByUserIdAsync(userId);
            var duePremiums = new List<object>();

            foreach (var policy in policies.Where(p => p.Status == Status.Active))
            {
                if (policy.NextPremiumDue.HasValue && policy.NextPremiumDue <= DateTime.Now.AddDays(30))
                {
                    duePremiums.Add(new
                    {
                        policyId = policy.PolicyId,
                        policyNumber = policy.PolicyNumber,
                        amount = policy.PremiumAmount,
                        dueDate = policy.NextPremiumDue,
                        daysOverdue = (DateTime.Now - policy.NextPremiumDue.Value).Days,
                        status = policy.NextPremiumDue <= DateTime.Now ? Status.Expired.ToString() : Status.Pending.ToString()
                    });
                }
            }

            return duePremiums;
        }

        public async Task<PaymentInitiateDto?> PayPremiumAsync(string policyId, string userId, string paymentMethod)
        {
            var policy = await _policyRepo.GetByIdAsync(policyId);
            if (policy == null || policy.UserId != userId || policy.Status != Status.Active) 
                return null;

            var transactionId = GenerateTransactionId();
            
          
            var payment = new Payment
            {
                PolicyId = policyId,
                UserId = policy.UserId,
                Amount = policy.PremiumAmount,
                PaymentType = PaymentType.Premium.ToString(),
                PaymentMethod = paymentMethod,
                TransactionId = transactionId,
                PaymentGateway = paymentMethod,
                Status = Status.Success.ToString()
            };

            await _repo.AddAsync(payment);

           
            var nextDue = CalculateNextDueDate(policy.PremiumFrequency);
            await _policyRepo.UpdateNextPremiumDueAsync(policyId, nextDue);

            return new PaymentInitiateDto
            {
                PolicyId = policyId,
                Amount = policy.PremiumAmount,
                PaymentMethod = Enum.Parse<PaymentMethod>(paymentMethod),
                ReturnUrl = $"/api/payment/receipt/{transactionId}"
            };
        }

        public async Task<PaymentStatusDto?> GetReceiptAsync(string transactionId)
        {
            var payment = await _repo.GetByTransactionIdAsync(transactionId);
            return payment == null ? null : _mapper.Map<PaymentStatusDto>(payment);
        }

        public async Task<PaymentStatusDto?> GetByIdAsync(string id)
        {
            var payment = await _repo.GetByIdAsync(id);
            return payment == null ? null : _mapper.Map<PaymentStatusDto>(payment);
        }

        private static string GenerateTransactionId()
        {
            const string transactionPrefix = "TXN";
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var randomSuffix = Random.Shared.Next(1000, 9999);
            return $"{transactionPrefix}{timestamp}{randomSuffix}";
        }

        public async Task<PaymentStatusDto?> PayProposalAsync(string proposalId, string userId, string paymentMethod)
        {
            try
            {
                Console.WriteLine($"PayProposalAsync called: proposalId={proposalId}, userId={userId}, paymentMethod={paymentMethod}");
                
                var proposal = await _proposalRepo.GetByIdAsync(proposalId);
                if (proposal == null)
                {
                    Console.WriteLine($"Proposal {proposalId} not found");
                    return null;
                }
                
                if (proposal.UserId != userId)
                {
                    Console.WriteLine($"Proposal {proposalId} does not belong to user {userId}");
                    return null;
                }
                
                if (proposal.Status != Status.Issued)
                {
                    Console.WriteLine($"Proposal {proposalId} status is {proposal.Status}, not Issued");
                    return null;
                }

                var transactionId = GenerateTransactionId();
                Console.WriteLine($"Generated transaction ID: {transactionId}");
                
                // Create payment record
                var payment = new Payment
                {
                    PolicyId = null, // Will be set after policy creation
                    ProposalId = proposalId,
                    UserId = proposal.UserId,
                    Amount = proposal.PremiumAmount,
                    PaymentType = PaymentType.Premium.ToString(),
                    PaymentMethod = paymentMethod,
                    TransactionId = transactionId,
                    PaymentGateway = paymentMethod,
                    Status = Status.Success.ToString()
                };

                Console.WriteLine($"Creating payment record...");
                var createdPayment = await _repo.AddAsync(payment);
                Console.WriteLine($"Payment created with ID: {createdPayment.PaymentId}");

                // Convert proposal to policy after successful payment
                Console.WriteLine($"Converting proposal to policy...");
                await ConvertProposalToPolicyAsync(proposalId);

                Console.WriteLine($"Mapping payment to DTO...");
                return _mapper.Map<PaymentStatusDto>(createdPayment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PayProposalAsync: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        public async Task<IEnumerable<object>> GetPendingPaymentsAsync(string userId)
        {
            // Get issued proposals that haven't been paid yet
            var issuedProposals = await _proposalRepo.GetByUserIdAsync(userId);
            var pendingPayments = new List<object>();

            foreach (var proposal in issuedProposals.Where(p => p.Status == Status.Issued))
            {
                // Check if payment already exists for this proposal
                var existingPayment = await _repo.GetByProposalIdAsync(proposal.ProposalId);
                if (existingPayment == null)
                {
                    pendingPayments.Add(new
                    {
                        proposalId = proposal.ProposalId,
                        productId = proposal.ProductId,
                        amount = proposal.PremiumAmount,
                        sumAssured = proposal.SumAssured,
                        issuedDate = proposal.ReviewedDate,
                        status = Status.Pending.ToString(),
                        type = PaymentType.Premium.ToString()
                    });
                }
            }

            return pendingPayments;
        }

        public async Task<IEnumerable<object>> GetIssuedProposalsAsync(string userId)
        {
            var proposals = await _proposalRepo.GetByUserIdAsync(userId);
            return proposals.Where(p => p.Status == Status.Issued).Select(p => new
            {
                proposalId = p.ProposalId,
                productId = p.ProductId,
                premiumAmount = p.PremiumAmount,
                sumAssured = p.SumAssured,
                issuedDate = p.ReviewedDate,
                status = Status.Issued.ToString()
            });
        }

        private async Task ConvertProposalToPolicyAsync(string proposalId)
        {
            try
            {
                Console.WriteLine($"Converting proposal {proposalId} to policy...");
                
                var proposal = await _proposalRepo.GetByIdAsync(proposalId);
                if (proposal == null)
                {
                    Console.WriteLine($"Proposal {proposalId} not found for conversion");
                    return;
                }

                // Generate policy number
                var lastPolicy = await _policyRepo.GetLastPolicyAsync();
                var policyNumber = GeneratePolicyNumber(lastPolicy?.PolicyNumber);
                Console.WriteLine($"Generated policy number: {policyNumber}");

                // Create new policy from proposal
                var policy = new Policy
                {
                    UserId = proposal.UserId,
                    ProductId = proposal.ProductId,
                    PolicyNumber = policyNumber,
                    PolicyType = PolicyType.TermLife,
                    SumAssured = proposal.SumAssured,
                    PremiumAmount = proposal.PremiumAmount,
                    PremiumFrequency = proposal.PremiumFrequency,
                    TermYears = proposal.TermYears,
                    StartDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(proposal.TermYears),
                    Status = Status.Active,
                    NextPremiumDue = CalculateNextDueDate(proposal.PremiumFrequency)
                };

                Console.WriteLine($"Creating policy with details: UserId={policy.UserId}, ProductId={policy.ProductId}");
                var createdPolicy = await _policyRepo.AddAsync(policy);
                Console.WriteLine($"Policy created with ID: {createdPolicy.PolicyId}");

                // Update payment record with actual PolicyId
                var payment = await _repo.GetByProposalIdAsync(proposalId);
                if (payment != null)
                {
                    payment.PolicyId = createdPolicy.PolicyId;
                    await _repo.UpdateAsync(payment);
                    Console.WriteLine($"Payment record updated with PolicyId: {createdPolicy.PolicyId}");
                }

                // Update proposal status to indicate it's been converted
                proposal.Status = Status.Success;
                await _proposalRepo.UpdateAsync(proposal);
                Console.WriteLine($"Proposal {proposalId} status updated to Success");

                Console.WriteLine($"SUCCESS: Proposal {proposalId} converted to policy {createdPolicy.PolicyId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR converting proposal to policy: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private static string GeneratePolicyNumber(string? lastPolicyNumber)
        {
            const string policyPrefix = "POL";
            const int policyNumberLength = 3;
            
            if (string.IsNullOrEmpty(lastPolicyNumber) || !lastPolicyNumber.StartsWith(policyPrefix))
                return $"{policyPrefix}{1.ToString($"D{policyNumberLength}")}";

            try
            {
                var numPart = lastPolicyNumber.Substring(policyPrefix.Length);
                var nextNum = int.Parse(numPart) + 1;
                return $"{policyPrefix}{nextNum.ToString($"D{policyNumberLength}")}";
            }
            catch
            {
                // If parsing fails, start from 1
                return $"{policyPrefix}{1.ToString($"D{policyNumberLength}")}";
            }
        }

        private static DateTime CalculateNextDueDate(PremiumFrequency frequency)
        {
            return frequency switch
            {
                PremiumFrequency.Monthly => DateTime.Now.AddMonths(1),
                PremiumFrequency.Quarterly => DateTime.Now.AddMonths(3),
                PremiumFrequency.Annual => DateTime.Now.AddYears(1),
                _ => DateTime.Now.AddYears(1)
            };
        }

    }
}
