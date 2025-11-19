using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class UnderwritingCaseDto
    {
        public int CaseId { get; set; }
        public int ProposalId { get; set; }
        public int UnderwriterId { get; set; }
        public RiskLevel RiskLevel { get; set; }
        public string? Comments { get; set; }
        public Status Status { get; set; }
        public DateTime AssignedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int UserId { get; set; }
        public string CaseType { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public int DaysPending { get; set; }
    }

    public class CaseDecisionDto
    {
        public Status Status { get; set; }
        public string? Comments { get; set; }
        public int CaseId { get; set; }
        public string Decision { get; set; } = string.Empty;
        public string DecisionReason { get; set; } = string.Empty;
        public decimal ApprovedSumAssured { get; set; }
    }

    public class AssignedCustomerDto
    {
        public string UserId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string KycStatus { get; set; } = string.Empty;
        public DateTime AssignedAt { get; set; }
        public int PendingDocuments { get; set; }
    }
}
