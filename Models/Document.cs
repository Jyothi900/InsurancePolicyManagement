using System.ComponentModel.DataAnnotations;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Models
{
    public class Document
    {
        [Key]
        public string DocumentId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        public string? ProposalId { get; set; }
        public string? PolicyId { get; set; }
        public string? ClaimId { get; set; }

        [Required]
        public DocumentType DocumentType { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9._\-]+\.(pdf|jpg|jpeg|png|doc|docx)$")]
        public string? FileName { get; set; }

        [Required]
        [StringLength(500)]
        public string? FilePath { get; set; }

        public string? FileHash { get; set; }

        [Required]
        public Status Status { get; set; } = Status.Uploaded;

        public string? VerificationNotes { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
        public DateTime? VerifiedAt { get; set; }

        public User? User { get; set; }
        public Proposal? Proposal { get; set; }
        public Policy? Policy { get; set; }
        public Claim? Claim { get; set; }
    }
}
