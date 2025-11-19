using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IUser
    {
        Task<User> GetByUsernameAsync(string username);
        Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail);
        Task<bool> ResetPasswordAsync(string usernameOrEmail, string newPassword);


    }
}
