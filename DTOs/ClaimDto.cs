using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class ClaimCreateDto
    {
        public string PolicyId { get; set; } = string.Empty;
        public AllClaimTypes ClaimType { get; set; }
        public DateTime IncidentDate { get; set; }
        public CauseOfDeath CauseOfDeath { get; set; }
        public string ClaimantName { get; set; } = string.Empty;
        public UnifiedRelationship ClaimantRelation { get; set; }
        public string ClaimantBankDetails { get; set; } = string.Empty;
    }

    public class ClaimDto
    {
        public string ClaimId { get; set; } = string.Empty;
        public string ClaimNumber { get; set; } = string.Empty;
        public string PolicyId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? AgentId { get; set; }
        public string? UnderwriterId { get; set; }
        public AllClaimTypes ClaimType { get; set; }
        public decimal ClaimAmount { get; set; }
        public DateTime IncidentDate { get; set; }
        public Status Status { get; set; }
        public DateTime FiledDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public CauseOfDeath CauseOfDeath { get; set; }
        public string ClaimantName { get; set; } = string.Empty;
        public UnifiedRelationship ClaimantRelation { get; set; }
        public string ClaimantBankDetails { get; set; } = string.Empty;
    }

    public class ClaimStatusDto
    {
        public string ClaimId { get; set; } = string.Empty;
        public string ClaimNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? InvestigationNotes { get; set; }
        public decimal? ApprovedAmount { get; set; }
        public DateTime FiledDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
    }
}
