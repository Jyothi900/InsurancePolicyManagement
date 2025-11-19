using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class NomineeDto
    {
        public string NomineeId { get; set; } = string.Empty;
        public string? PolicyId { get; set; }
        public string? ProposalId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public UnifiedRelationship Relationship { get; set; }
        public decimal SharePercentage { get; set; }
        public string? AadhaarNumber { get; set; }
        public string? Address { get; set; }
    }
}
