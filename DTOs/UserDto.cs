using InsurancePolicyManagement.Enums;
using Microsoft.AspNetCore.Http;

namespace InsurancePolicyManagement.DTOs
{
    public class UserDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string? ProfileImagePath { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? AadhaarNumber { get; set; }
        public string? PANNumber { get; set; }
        public string Address { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public KYCStatus KYCStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AssignedAgentId { get; set; }
        public DateTime? AgentAssignedDate { get; set; }
        public string? AssignedUnderwriterId { get; set; }
        public DateTime? UnderwriterAssignedDate { get; set; }
    }

    public class UserRegistrationDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Customer;
    }

    public class UserUpdateDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? AadhaarNumber { get; set; }
        public string? PANNumber { get; set; }
        public string? Address { get; set; }
        public KYCStatus KYCStatus { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AgentAssignmentDto
    {
        public string AgentId { get; set; } = string.Empty;
        public string? UnderwriterId { get; set; }
    }

    public class AgentAssignmentResponseDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string AgentId { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
        public string AgentContact { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ProfileImageUploadDto
    {
        public string UserId { get; set; } = string.Empty;
        public IFormFile ProfileImage { get; set; } = null!;
    }
}
