using Microsoft.EntityFrameworkCore;
using InsurancePolicyManagement.Data;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly InsuranceManagementContext _context;

        public UserRepository(InsuranceManagementContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            // Only return active users
            return await _context.Users.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
        }

        public async Task<User?> GetByMobileAsync(string mobile)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Mobile == mobile);
        }

        public async Task<User> AddAsync(User user)
        {
            var lastUser = await _context.Users
                                     .OrderByDescending(u => u.UserId)
                                     .FirstOrDefaultAsync();

            string newId = lastUser == null
                ? "USE001"
                : "USE" + (int.Parse(lastUser.UserId.Substring(3)) + 1).ToString("D3");

            user.UserId = newId;
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (existing == null) return false;

            // Soft delete - mark as inactive instead of removing
            existing.IsActive = false;
            _context.Users.Update(existing);
            await _context.SaveChangesAsync();
            return true;
        }



        public async Task<IEnumerable<User>> GetUnassignedCustomersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Customer && u.AssignedAgentId == null && u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAgentsAsync(string? location = null)
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Agent && u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUnderwritersAsync()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Underwriter && u.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetCustomersWithAssignmentsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == UserRole.Customer && u.IsActive)
                .ToListAsync();
        }
    }
}
