using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(string id);
        Task<User?> GetByEmailAsync(string email);
        //Task<User?> GetByMobileAsync(string mobile);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(string id);

        Task<IEnumerable<User>> GetUnassignedCustomersAsync();
        Task<IEnumerable<User>> GetAgentsAsync(string? location = null);
        Task<IEnumerable<User>> GetUnderwritersAsync();
        Task<IEnumerable<User>> GetCustomersWithAssignmentsAsync();
    }
}
