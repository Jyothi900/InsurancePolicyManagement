using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Enums;
using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IPolicyService _policyService;
        private readonly IClaimService _claimService;
        private readonly IPaymentService _paymentService;
        private readonly IProposalService _proposalService;
        private readonly IPolicyRepository _policyRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IUserRepository _userRepo;
        private readonly IProposalRepository _proposalRepo;
        private readonly IDocumentRepository _documentRepo;

        public DashboardService(
            IPolicyService policyService,
            IClaimService claimService,
            IPaymentService paymentService,
            IProposalService proposalService,
            IPolicyRepository policyRepo,
            IClaimRepository claimRepo,
            IPaymentRepository paymentRepo,
            IUserRepository userRepo,
            IProposalRepository proposalRepo,
            IDocumentRepository documentRepo)
        {
            _policyService = policyService;
            _claimService = claimService;
            _paymentService = paymentService;
            _proposalService = proposalService;
            _policyRepo = policyRepo;
            _claimRepo = claimRepo;
            _paymentRepo = paymentRepo;
            _userRepo = userRepo;
            _proposalRepo = proposalRepo;
            _documentRepo = documentRepo;
        }

        public async Task<DashboardData> GetCustomerDashboardAsync(string userId)
        {
            var policies = await _policyService.GetByUserIdAsync(userId);
            var claims = await _claimService.GetByUserIdAsync(userId);
            var duePremiums = await _paymentService.GetDuePremiumsAsync(userId);

            return new DashboardData
            {
                Policies = policies,
                Claims = claims,
                DuePremiums = duePremiums,
                Summary = new DashboardSummary
                {
                    TotalCoverage = policies.Sum(p => p.SumAssured),
                    ActivePolicies = policies.Count(p => p.Status == Status.Active),
                    PendingClaims = claims.Count(c => c.Status == Status.Pending),
                    DuePayments = duePremiums.Count()
                }
            };
        }

        public async Task<object> GetTestDataAsync()
        {
            var userCount = await _userRepo.GetAllAsync();
            var policyCount = await _policyRepo.GetAllAsync();
            var claimCount = await _claimRepo.GetAllAsync();
            var paymentCount = await _paymentRepo.GetAllAsync();
            
            var proposalServiceCount = 0;
            var claimServiceCount = 0;
            
            try
            {
                var proposals = await _proposalService.GetAllAsync();
                proposalServiceCount = proposals.Count();
            }
            catch (Exception)
            {
            }
            
            try
            {
                var claims = await _claimService.GetAllAsync();
                claimServiceCount = claims.Count();
            }
            catch (Exception)
            {
            }
            
            return new { 
                message = "Dashboard controller is working", 
                timestamp = DateTime.Now,
                databaseConnected = true,
                counts = new {
                    users = userCount.Count(),
                    policies = policyCount.Count(),
                    claims = claimCount.Count(),
                    payments = paymentCount.Count(),
                    proposalsFromService = proposalServiceCount,
                    claimsFromService = claimServiceCount
                }
            };
        }

        public async Task<object> GetAdminDashboardAsync()
        {
            var allUsers = new List<User>();
            var allPolicies = new List<Policy>();
            var allClaims = new List<Claim>();
            var allPayments = new List<Payment>();
            
            try
            {
                allUsers = (await _userRepo.GetAllAsync()).ToList();
            }
            catch (Exception)
            {
            }
            
            try
            {
                allPolicies = (await _policyRepo.GetAllAsync()).ToList();
            }
            catch (Exception)
            {
            }
            
            try
            {
                allClaims = (await _claimRepo.GetAllAsync()).ToList();
            }
            catch (Exception)
            {
            }
            
            try
            {
                allPayments = (await _paymentRepo.GetAllAsync()).ToList();
            }
            catch (Exception)
            {
            }

            return new
            {
                totalUsers = allUsers.Count,
                totalPolicies = allPolicies.Count,
                totalClaims = allClaims.Count,
                pendingClaims = allClaims.Count(c => c.Status == "Pending"),
                approvedClaims = allClaims.Count(c => c.Status == "Approved"),
                totalRevenue = allPayments.Sum(p => p.Amount),
                activePolicies = allPolicies.Count(p => p.Status == Status.Active)
            };
        }

        public async Task<object> GetAgentDashboardAsync(string agentId)
        {
            var allPolicies = await _policyRepo.GetAllAsync();
            var allClaims = await _claimRepo.GetAllAsync();
            
            var agentPolicies = allPolicies.Where(p => p.AgentId == agentId);
            var agentClaims = allClaims.Where(c => c.AgentId == agentId);

            return new
            {
                totalPolicies = agentPolicies.Count(),
                activePolicies = agentPolicies.Count(p => p.Status == Status.Active),
                totalClaims = agentClaims.Count(),
                pendingClaims = agentClaims.Count(c => c.Status == "Pending"),
                commission = agentPolicies.Sum(p => p.PremiumAmount * 0.1m)
            };
        }

        public async Task<object> GetUnderwriterDashboardAsync(string underwriterId)
        {
            var allUsers = (await _userRepo.GetAllAsync()).ToList();
            var allProposals = (await _proposalRepo.GetAllAsync()).ToList();
            var allDocuments = (await _documentRepo.GetAllAsync()).ToList();
            var allClaims = (await _claimRepo.GetAllAsync()).ToList();
            var allPolicies = (await _policyRepo.GetAllAsync()).ToList();

            var assignedCustomers = allUsers.Where(u => 
                u.AssignedUnderwriterId == underwriterId && 
                u.Role == UserRole.Customer
            ).Select(u => new {
                customerId = u.UserId,
                customerName = u.FullName,
                customerEmail = u.Email,
                kycStatus = (int)u.KYCStatus,
                assignedDate = u.UnderwriterAssignedDate?.ToString("yyyy-MM-dd") ?? "N/A"
            }).ToList();

            var pendingProposals = allProposals.Select(p => new {
                proposalId = p.ProposalId,
                userId = p.UserId,
                productId = p.ProductId,
                sumAssured = p.SumAssured,
                premiumAmount = p.PremiumAmount,
                premiumFrequency = (int)p.PremiumFrequency,
                status = (int)p.Status,
                appliedDate = p.AppliedDate.ToString("yyyy-MM-dd"),
                reviewedDate = p.ReviewedDate?.ToString("yyyy-MM-dd"),
                isSmoker = p.IsSmoker,
                termYears = p.TermYears,
                height = p.Height,
                weight = p.Weight,
                isDrinker = p.IsDrinker,
                preExistingConditions = p.PreExistingConditions,
                occupation = p.Occupation,
                annualIncome = p.AnnualIncome
            }).ToList();

            var pendingDocuments = allDocuments.Select(d => new {
                documentId = d.DocumentId,
                userId = d.UserId,
                proposalId = d.ProposalId,
                policyId = d.PolicyId,
                claimId = d.ClaimId,
                documentType = (int)d.DocumentType,
                fileName = d.FileName,
                filePath = d.FilePath,
                status = (int)d.Status,
                uploadedDate = d.UploadedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                verifiedDate = d.VerifiedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
                notes = d.VerificationNotes
            }).ToList();

            var pendingClaims = allClaims.Select(c => new {
                claimId = c.ClaimId,
                claimNumber = c.ClaimNumber,
                policyId = c.PolicyId,
                userId = c.UserId,
                agentId = c.AgentId,
                underwriterId = c.UnderwriterId,
                claimType = c.ClaimType,
                claimAmount = c.ClaimAmount,
                incidentDate = c.IncidentDate.ToString("yyyy-MM-dd"),
                status = c.Status,
                filedDate = c.FiledDate.ToString("yyyy-MM-dd"),
                processedDate = c.ProcessedDate?.ToString("yyyy-MM-dd"),
                approvedAmount = c.ApprovedAmount,
                causeOfDeath = c.CauseOfDeath,
                claimantName = c.ClaimantName,
                claimantRelation = c.ClaimantRelation,
                claimantBankDetails = c.ClaimantBankDetails
            }).ToList();

            var enumsData = new {
                claimTypes = Enum.GetValues<AllClaimTypes>().Select(ct => new { value = (int)ct, name = ct.ToString() }).ToList(),
                causeOfDeath = Enum.GetValues<CauseOfDeath>().Select(cod => new { value = (int)cod, name = cod.ToString() }).ToList(),
                relationships = Enum.GetValues<UnifiedRelationship>().Select(r => new { value = (int)r, name = r.ToString() }).ToList(),
                premiumFrequencies = Enum.GetValues<PremiumFrequency>().Select(pf => new { value = (int)pf, name = pf.ToString() }).ToList(),
                paymentMethods = Enum.GetValues<PaymentMethod>().Select(pm => new { value = (int)pm, name = pm.ToString() }).ToList(),
                statuses = Enum.GetValues<Status>().Select(s => new { value = (int)s, name = s.ToString() }).ToList(),
                kycStatuses = Enum.GetValues<KYCStatus>().Select(ks => new { value = (int)ks, name = ks.ToString() }).ToList(),
                documentTypes = Enum.GetValues<DocumentType>().Select(dt => new { value = (int)dt, name = dt.ToString() }).ToList(),
                genders = Enum.GetValues<Gender>().Select(g => new { value = (int)g, name = g.ToString() }).ToList(),
                userRoles = Enum.GetValues<UserRole>().Select(ur => new { value = (int)ur, name = ur.ToString() }).ToList()
            };

            return new
            {
                assignedCustomers = assignedCustomers,
                proposals = pendingProposals,
                documents = pendingDocuments,
                claims = pendingClaims,
                enums = enumsData,
                summary = new {
                    totalAssignedCustomers = assignedCustomers.Count,
                    totalPendingProposals = pendingProposals.Count,
                    totalPendingDocuments = pendingDocuments.Count,
                    totalPendingClaims = pendingClaims.Count
                }
            };
        }
    }
}