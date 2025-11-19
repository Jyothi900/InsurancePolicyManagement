namespace InsurancePolicyManagement.DTOs
{
    public class PolicyByUserRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class PolicyByIdRequest
    {
        public string PolicyId { get; set; } = string.Empty;
    }

    public class PolicyActionRequest
    {
        public string PolicyId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }

    public class IssueFromProposalRequest
    {
        public string ProposalId { get; set; } = string.Empty;
    }
}