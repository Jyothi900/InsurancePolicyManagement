using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Enums;
using InsurancePolicyManagement.Models;
using static InsurancePolicyManagement.Enums.UserRole;

namespace InsurancePolicyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IPolicyService _policyService;
        private readonly IClaimService _claimService;
        private readonly IPaymentService _paymentService;
        private readonly IPolicyRepository _policyRepo;
        private readonly IClaimRepository _claimRepo;
        private readonly IPaymentRepository _paymentRepo;
        private readonly IUserRepository _userRepo;

        public DashboardController(
            IPolicyService policyService,
            IClaimService claimService,
            IPaymentService paymentService,
            IPolicyRepository policyRepo,
            IClaimRepository claimRepo,
            IPaymentRepository paymentRepo,
            IUserRepository userRepo)
        {
            _policyService = policyService;
            _claimService = claimService;
            _paymentService = paymentService;
            _policyRepo = policyRepo;
            _claimRepo = claimRepo;
            _paymentRepo = paymentRepo;
            _userRepo = userRepo;
        }

        [HttpPost("customer-data")]
        public async Task<IActionResult> GetCustomerDashboard([FromBody] CustomerDashboardRequest request)
        {
            try
            {
                var policies = await _policyService.GetByUserIdAsync(request.UserId);
                var claims = await _claimService.GetByUserIdAsync(request.UserId);
                var duePremiums = await _paymentService.GetDuePremiumsAsync(request.UserId);

                var response = new DashboardData
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

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<IActionResult> TestEndpoint()
        {
            try
            {
                var userCount = await _userRepo.GetAllAsync();
                var policyCount = await _policyRepo.GetAllAsync();
                var claimCount = await _claimRepo.GetAllAsync();
                var paymentCount = await _paymentRepo.GetAllAsync();
                
                return Ok(new { 
                    message = "Dashboard controller is working", 
                    timestamp = DateTime.Now,
                    databaseConnected = true,
                    counts = new {
                        users = userCount.Count(),
                        policies = policyCount.Count(),
                        claims = claimCount.Count(),
                        payments = paymentCount.Count()
                    }
                });
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    message = "Dashboard controller is working but database error", 
                    timestamp = DateTime.Now,
                    databaseConnected = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("admin-data")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetAdminDashboard()
        {
            try
            {
                Console.WriteLine(" Admin Dashboard API called");
                
              
                var allUsers = new List<User>();
                var allPolicies = new List<Policy>();
                var allClaims = new List<Claim>();
                var allPayments = new List<Payment>();
                
                try
                {
                    allUsers = (await _userRepo.GetAllAsync()).ToList();
                   
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error loading users: {ex.Message}");
                }
                
                try
                {
                    allPolicies = (await _policyRepo.GetAllAsync()).ToList();
                    Console.WriteLine($" Policies loaded: {allPolicies.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error loading policies: {ex.Message}");
                }
                
                try
                {
                    allClaims = (await _claimRepo.GetAllAsync()).ToList();
                    Console.WriteLine($" Claims loaded: {allClaims.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error loading claims: {ex.Message}");
                }
                
                try
                {
                    allPayments = (await _paymentRepo.GetAllAsync()).ToList();
                    Console.WriteLine($" Payments loaded: {allPayments.Count}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Error loading payments: {ex.Message}");
                }

                var response = new
                {
                    totalUsers = allUsers.Count,
                    totalPolicies = allPolicies.Count,
                    totalClaims = allClaims.Count,
                    pendingClaims = allClaims.Count(c => c.Status == "Pending"),
                    approvedClaims = allClaims.Count(c => c.Status == "Approved"),
                    totalRevenue = allPayments.Sum(p => p.Amount),
                    activePolicies = allPolicies.Count(p => p.Status == Status.Active)
                };

                Console.WriteLine($" Admin Dashboard Response: {System.Text.Json.JsonSerializer.Serialize(response)}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Admin Dashboard Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("agent-data")]
        public async Task<IActionResult> GetAgentDashboard([FromBody] AgentDashboardRequest request)
        {
            try
            {
                var allPolicies = await _policyRepo.GetAllAsync();
                var allClaims = await _claimRepo.GetAllAsync();
                
                var agentPolicies = allPolicies.Where(p => p.AgentId == request.AgentId);
                var agentClaims = allClaims.Where(c => c.AgentId == request.AgentId);

                var response = new
                {
                    totalPolicies = agentPolicies.Count(),
                    activePolicies = agentPolicies.Count(p => p.Status == Status.Active),
                    totalClaims = agentClaims.Count(),
                    pendingClaims = agentClaims.Count(c => c.Status == "Pending"),
                    commission = agentPolicies.Sum(p => p.PremiumAmount * 0.1m)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("underwriter-data")]
        public async Task<IActionResult> GetUnderwriterDashboard([FromBody] UnderwriterDashboardRequest request)
        {
            try
            {
                var allUsers = await _userRepo.GetAllAsync();

                var assignedCustomers = allUsers.Where(u => 
                    u.AssignedUnderwriterId == request.UnderwriterId && 
                    u.Role == UserRole.Customer
                ).Select(u => new {
                    customerId = u.UserId,
                    customerName = u.FullName,
                    customerEmail = u.Email,
                    kycStatus = (int)u.KYCStatus,
                    assignedDate = u.UnderwriterAssignedDate?.ToString("yyyy-MM-dd") ?? "N/A"
                }).ToList();

                var response = new
                {
                    assignedCustomers = assignedCustomers
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }

}