using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Services
{
    public class UserAuthService : IUser
    {
        private readonly IUserRepository _userRepository;

        public UserAuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _userRepository.GetByEmailAsync(username);
        }

        public async Task<User?> GetByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _userRepository.GetByEmailAsync(usernameOrEmail);
        }

        public async Task<bool> ResetPasswordAsync(string usernameOrEmail, string newPassword)
        {
            var user = await _userRepository.GetByEmailAsync(usernameOrEmail);
            if (user == null) return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
