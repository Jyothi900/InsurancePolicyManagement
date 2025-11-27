using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardData> GetCustomerDashboardAsync(string userId);
        Task<object> GetAdminDashboardAsync();
        Task<object> GetAgentDashboardAsync(string agentId);
        Task<object> GetUnderwriterDashboardAsync(string underwriterId);
        Task<object> GetTestDataAsync();
    }
}