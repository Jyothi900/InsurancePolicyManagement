
namespace InsurancePolicyManagement.DTOs
{
    public class CustomerDashboardRequest
    {
        public string? UserId { get; set; }
    }

    public class AgentDashboardRequest
    {
        public string? AgentId { get; set; }
    }

    public class UnderwriterDashboardRequest
    {
        public string? UnderwriterId { get; set; }
    }

    public class DashboardData
    {
        public IEnumerable<PolicyDTO>? Policies { get; set; }
        public IEnumerable<ClaimDto>? Claims { get; set; }
        public IEnumerable<object>? DuePremiums { get; set; }
        public DashboardSummary? Summary { get; set; }
    }

    public class DashboardSummary
    {
        public decimal TotalCoverage { get; set; }
        public int ActivePolicies { get; set; }
        public int PendingClaims { get; set; }
        public int DuePayments { get; set; }
    }
}