using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.DTOs
{
    public class DocumentDto
    {
        public string DocumentId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string? PolicyId { get; set; }
        public string? ProposalId { get; set; }
        public string? ClaimId { get; set; }
        public DocumentType DocumentType { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; }
        public Status Status { get; set; }
    }

    public class DocumentUploadDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? PolicyId { get; set; }
        public string? ProposalId { get; set; }
        public string? ClaimId { get; set; }
        public IFormFile File { get; set; } = null!;
    }

    public class DocumentVerificationDto
    {
        public string VerificationId { get; set; } = string.Empty;
        public string DocumentId { get; set; } = string.Empty;
        public string? UnderwriterId { get; set; }
        public bool IsVerified { get; set; }
        public string VerificationStatus { get; set; } = "Pending";
        public string? VerificationNotes { get; set; }
        public decimal ConfidenceScore { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }

    public class DocumentDecisionDto
    {
        public string VerificationId { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string VerificationStatus { get; set; } = "Pending";
        public string VerificationNotes { get; set; } = string.Empty;
        public decimal ConfidenceScore { get; set; }
    }

    public class KYCDocumentUploadDto
    {
        public string UserId { get; set; } = string.Empty;
        

        public IFormFile? AadhaarFile { get; set; }
        public string AadhaarNumber { get; set; } = string.Empty;
        
       
        public IFormFile? PANFile { get; set; }
        public string PANNumber { get; set; } = string.Empty;
      
        public string BankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string IFSCCode { get; set; } = string.Empty;
        public string AccountHolderName { get; set; } = string.Empty;
    }

    public class KYCUploadResponseDto
    {
        public string Message { get; set; } = string.Empty;
        public List<string> UploadedDocuments { get; set; } = new();
        public string KYCStatus { get; set; } = "Pending";
    }

    public class SimpleDocumentVerificationDto
    {
        public string DocumentId { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public string VerificationNotes { get; set; } = string.Empty;
    }
}
