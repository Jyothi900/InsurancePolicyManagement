using System.ComponentModel.DataAnnotations;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [RegularExpression(@"^[a-zA-Z\s.]+$")]
        public string? FullName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Customer;

        [Required]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be exactly 10 digits")]
        public string? Mobile { get; set; }

        public string? ProfileImagePath { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [StringLength(12, MinimumLength = 12)]
        [RegularExpression(@"^[0-9]{12}$")]
        public string? AadhaarNumber { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [RegularExpression(@"^[A-Z]{5}[0-9]{4}[A-Z]{1}$")]
        public string? PANNumber { get; set; }

        public string? Address { get; set; }

        [Required]
        public KYCStatus KYCStatus { get; set; } = KYCStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = true;

        // OTP fields for password reset
        public string? OtpCode { get; set; }
        public DateTime? OtpExpiry { get; set; }
      
        public string? AssignedAgentId { get; set; }
        public DateTime? AgentAssignedDate { get; set; }
        
        public string? AssignedUnderwriterId { get; set; }
        public DateTime? UnderwriterAssignedDate { get; set; }

        
        public ICollection<Policy>? Policies { get; set; }
        public ICollection<Proposal>? Proposals { get; set; }
        public ICollection<Claim>? Claims { get; set; }
        public ICollection<Payment>? Payments { get; set; }
        public ICollection<Document>? Documents { get; set; }
        public ICollection<UnderwritingCase>? UnderwritingCases { get; set; }
    }
}
