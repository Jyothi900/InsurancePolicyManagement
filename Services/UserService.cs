using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Enums;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace InsurancePolicyManagement.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto?> CreateAsync(UserRegistrationDto dto)
        {
            
            var existingUser = await _repo.GetByEmailAsync(dto.Email);
            if (existingUser != null) return null;

            var user = _mapper.Map<User>(dto);
            // Role is now mapped from DTO, no need to hardcode
            user.KYCStatus = KYCStatus.Pending;

            var created = await _repo.AddAsync(user);
            return _mapper.Map<UserDto>(created);
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _repo.GetByEmailAsync(email);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetByIdAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id);
            return user == null ? null : _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> UpdateAsync(string id, UserUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            // Update only provided fields (partial update)
            if (!string.IsNullOrEmpty(dto.FullName))
                existing.FullName = dto.FullName;
            
            if (!string.IsNullOrEmpty(dto.Email))
                existing.Email = dto.Email;
            
            existing.Role = dto.Role;
            
            if (!string.IsNullOrEmpty(dto.Mobile))
                existing.Mobile = dto.Mobile;
            
            existing.DateOfBirth = DateOnly.FromDateTime(dto.DateOfBirth);
            
            existing.Gender = dto.Gender;
            
            if (!string.IsNullOrEmpty(dto.AadhaarNumber))
                existing.AadhaarNumber = dto.AadhaarNumber;
            
            if (!string.IsNullOrEmpty(dto.PANNumber))
                existing.PANNumber = dto.PANNumber;
            
            if (!string.IsNullOrEmpty(dto.Address))
                existing.Address = dto.Address;
            
            existing.KYCStatus = dto.KYCStatus;
            
            await _repo.UpdateAsync(existing);
            return _mapper.Map<UserDto>(existing);
        }



       

        public async Task<bool> DeleteAsync(string id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<AgentAssignmentResponseDto?> AssignAgentAsync(string customerId, string agentId, string? underwriterId = null)
        {
            var customer = await _repo.GetByIdAsync(customerId);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {customerId} not found");
            if (customer.Role != UserRole.Customer)
                throw new ArgumentException($"User {customerId} is not a customer");

            var agent = await _repo.GetByIdAsync(agentId);
            if (agent == null)
                throw new ArgumentException($"Agent with ID {agentId} not found");
            if (agent.Role != UserRole.Agent)
                throw new ArgumentException($"User {agentId} is not an agent");

            var assignedDate = DateTime.Now;
            customer.AssignedAgentId = agentId;
            customer.AgentAssignedDate = assignedDate;
            
            // Assign underwriter if provided
            if (!string.IsNullOrEmpty(underwriterId))
            {
                var underwriter = await _repo.GetByIdAsync(underwriterId);
                if (underwriter == null)
                    throw new ArgumentException($"Underwriter with ID {underwriterId} not found");
                if (underwriter.Role != UserRole.Underwriter)
                    throw new ArgumentException($"User {underwriterId} is not an underwriter");
                    
                customer.AssignedUnderwriterId = underwriterId;
                customer.UnderwriterAssignedDate = assignedDate;
            }

            await _repo.UpdateAsync(customer);

            return new AgentAssignmentResponseDto
            {
                CustomerId = customer.UserId,
                CustomerName = customer.FullName ?? string.Empty,
                AgentId = agent.UserId,
                AgentName = agent.FullName ?? string.Empty,
                AgentContact = agent.Mobile ?? string.Empty,
                AssignedDate = assignedDate,
                Message = "Agent and underwriter assigned successfully"
            };
        }

        public async Task<IEnumerable<UserDto>> GetUnassignedCustomersAsync()
        {
            var customers = await _repo.GetUnassignedCustomersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(customers);
        }

        public async Task<IEnumerable<UserDto>> GetAgentsAsync(string? location = null)
        {
            var agents = await _repo.GetAgentsAsync(location);
            return _mapper.Map<IEnumerable<UserDto>>(agents);
        }

        public async Task<IEnumerable<UserDto>> GetUnderwritersAsync()
        {
            var underwriters = await _repo.GetUnderwritersAsync();
            return _mapper.Map<IEnumerable<UserDto>>(underwriters);
        }

        public async Task<IEnumerable<CustomerAssignmentDto>> GetCustomerAssignmentsAsync()
        {
            var customers = await _repo.GetCustomersWithAssignmentsAsync();
            var result = new List<CustomerAssignmentDto>();
            
            foreach (var customer in customers)
            {
                var assignment = new CustomerAssignmentDto
                {
                    CustomerId = customer.UserId,
                    CustomerName = customer.FullName ?? string.Empty,
                    CustomerEmail = customer.Email,
                    AgentId = customer.AssignedAgentId,
                    UnderwriterId = customer.AssignedUnderwriterId,
                    AssignedDate = customer.AgentAssignedDate ?? customer.CreatedAt
                };
                
                if (!string.IsNullOrEmpty(customer.AssignedAgentId))
                {
                    var agent = await _repo.GetByIdAsync(customer.AssignedAgentId);
                    assignment.AgentName = agent?.FullName;
                }
                
                if (!string.IsNullOrEmpty(customer.AssignedUnderwriterId))
                {
                    var underwriter = await _repo.GetByIdAsync(customer.AssignedUnderwriterId);
                    assignment.UnderwriterName = underwriter?.FullName;
                }
                
                result.Add(assignment);
            }
            
            return result;
        }

        public async Task<string> UploadProfileImageAsync(string userId, IFormFile profileImage)
        {
            var user = await _repo.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            // Validate file
            if (profileImage.Length == 0)
                throw new ArgumentException("Profile image file is empty");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(profileImage.FileName).ToLowerInvariant();
            
            if (!allowedExtensions.Contains(fileExtension))
                throw new ArgumentException("Only JPG, JPEG, PNG, and GIF files are allowed");

            if (profileImage.Length > 5 * 1024 * 1024) // 5MB limit
                throw new ArgumentException("Profile image must be less than 5MB");

            // Create uploads directory if it doesn't exist
            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profiles");
            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            // Generate unique filename
            var fileName = $"{userId}_{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsPath, fileName);

            // Delete old profile image if exists
            if (!string.IsNullOrEmpty(user.ProfileImagePath))
            {
                var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.ProfileImagePath.TrimStart('/'));
                if (File.Exists(oldImagePath))
                    File.Delete(oldImagePath);
            }

            // Save new image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }

            // Update user profile image path
            var relativePath = $"/uploads/profiles/{fileName}";
            user.ProfileImagePath = relativePath;
            await _repo.UpdateAsync(user);

            return relativePath;
        }

    }
}
