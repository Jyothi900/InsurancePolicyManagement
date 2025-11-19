using System.ComponentModel.DataAnnotations;

namespace InsurancePolicyManagement.Models
{
    public class DocumentVerification
    {
        [Key]
        public string VerificationId { get; set; } = string.Empty;
        
        [Required]
        public string DocumentId { get; set; } = string.Empty;
        
        public string? UnderwriterId { get; set; }
        
        public bool IsVerified { get; set; } = false;
        public string VerificationStatus { get; set; } = "Pending";
        public string? VerificationNotes { get; set; }
        public decimal ConfidenceScore { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? VerifiedAt { get; set; }
        
       
        public Document? Document { get; set; }
        public User? Underwriter { get; set; }
    }
}
