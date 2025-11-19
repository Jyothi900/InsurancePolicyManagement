namespace InsurancePolicyManagement.DTOs
{
    public class UserByIdRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class UserByEmailRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class UserDeleteRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class AgentsByLocationRequest
    {
        public string Location { get; set; } = string.Empty;
    }

    public class UserUpdateSecureRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Role { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public int Gender { get; set; }
        public string? AadhaarNumber { get; set; }
        public string? PanNumber { get; set; }
        public string? Address { get; set; }
        public int KycStatus { get; set; }
    }

    public class AgentAssignmentSecureRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string AgentId { get; set; } = string.Empty;
        public string? UnderwriterId { get; set; }
    }
}