using InsurancePolicyManagement.DTOs;
using Microsoft.AspNetCore.Http;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IUserService
    {
        Task<UserDto?> CreateAsync(UserRegistrationDto dto);
        Task<UserDto?> GetByIdAsync(string id);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<UserDto?> UpdateAsync(string id, UserUpdateDto dto);

        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<bool> DeleteAsync(string id);
        Task<AgentAssignmentResponseDto?> AssignAgentAsync(string customerId, string agentId, string? underwriterId = null);
        Task<IEnumerable<UserDto>> GetUnassignedCustomersAsync();
        Task<IEnumerable<UserDto>> GetAgentsAsync(string? location = null);
        Task<IEnumerable<UserDto>> GetUnderwritersAsync();
        Task<IEnumerable<CustomerAssignmentDto>> GetCustomerAssignmentsAsync();
        Task<string> UploadProfileImageAsync(string userId, IFormFile profileImage);
    }
}
