namespace InsurancePolicyManagement.DTOs
{
    // Claim DTOs
    public class ClaimByUserRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class ClaimFileRequest
    {
        public string UserId { get; set; } = string.Empty;
        public ClaimCreateDto ClaimData { get; set; } = new ClaimCreateDto();
    }

    public class ClaimStatusCheckRequest
    {
        public string ClaimNumber { get; set; } = string.Empty;
    }

    public class ClaimRequiredDocsRequest
    {
        public string ClaimNumber { get; set; } = string.Empty;
    }

    public class ClaimTimelineRequest
    {
        public string ClaimNumber { get; set; } = string.Empty;
    }

    public class ClaimStatusUpdateRequest
    {
        public string ClaimId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class ClaimApproveRequest
    {
        public string ClaimId { get; set; } = string.Empty;
        public decimal ApprovedAmount { get; set; }
    }

    // Proposal DTOs
    public class ProposalByIdRequest
    {
        public string ProposalId { get; set; } = string.Empty;
    }

    public class ProposalSubmitRequest
    {
        public string UserId { get; set; } = string.Empty;
        public ProposalCreateDto ProposalData { get; set; } = new ProposalCreateDto();
    }

    public class ProposalByUserRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class ProposalStatusUpdateRequest
    {
        public string ProposalId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    // Document DTOs
    public class DocumentByIdRequest
    {
        public string DocumentId { get; set; } = string.Empty;
    }

    public class DocumentByUserRequest
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class DocumentByProposalRequest
    {
        public string ProposalId { get; set; } = string.Empty;
    }

    public class DocumentByPolicyRequest
    {
        public string PolicyId { get; set; } = string.Empty;
    }

    public class DocumentByClaimRequest
    {
        public string ClaimId { get; set; } = string.Empty;
    }

    public class DocumentDeleteRequest
    {
        public string DocumentId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }

    // Product DTOs
    public class ProductByIdRequest
    {
        public string ProductId { get; set; } = string.Empty;
    }

    public class ProductByTypeRequest
    {
        public string InsuranceType { get; set; } = string.Empty;
    }

    public class ProductEligibilityRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    public class ProductUpdateRequest
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal MinSumAssured { get; set; }
        public decimal MaxSumAssured { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int MinTerm { get; set; }
        public int MaxTerm { get; set; }
    }

    public class ProductDeleteRequest
    {
        public string ProductId { get; set; } = string.Empty;
    }

    // Underwriter DTOs
    public class UnderwriterByIdRequest
    {
        public string UnderwriterId { get; set; } = string.Empty;
    }

    public class UnderwriterKYCUpdateRequest
    {
        public string UserId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}