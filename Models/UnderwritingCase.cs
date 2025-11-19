using System.ComponentModel.DataAnnotations;

namespace InsurancePolicyManagement.Models
{
    public class UnderwritingCase
    {
        [Key]
        public string CaseId { get; set; } = string.Empty;
        

        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public string ProductId { get; set; } = string.Empty;
        
     
        public string CaseType { get; set; } = string.Empty; 
        public string Priority { get; set; } = "Normal"; 
        public string Status { get; set; } = "Assigned"; 
        
       
        public string? AssignedUnderwriterId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public DateTime? CompletedAt { get; set; }
        
      
        public string? Decision { get; set; }
        public string? DecisionReason { get; set; }
        public decimal? ApprovedSumAssured { get; set; }
        public decimal? ApprovedPremium { get; set; }
        public string? ApprovalConditions { get; set; }
        
    
        public string? RequiredDocuments { get; set; }
        public string? UnderwriterNotes { get; set; }
        public string? RiskAssessment { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        

        public User? User { get; set; }
        public User? AssignedUnderwriter { get; set; }
    }
}
