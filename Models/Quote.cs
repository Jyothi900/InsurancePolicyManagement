using System.ComponentModel.DataAnnotations;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Models
{
    public class Quote
    {
        [Key]
        public string QuoteId { get; set; } = string.Empty;

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string ProductId { get; set; } = string.Empty;

        public string? AgentId { get; set; } 

        [Required]
        [Range(50000, 10000000)]
        public decimal SumAssured { get; set; }

        [Required]
        [Range(1, 100)]
        public int TermYears { get; set; }

        [Required]
        public decimal PremiumAmount { get; set; }

        [Required]
        public PremiumFrequency PremiumFrequency { get; set; } = PremiumFrequency.Annual;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ExpiryDate { get; set; } = DateTime.Now.AddDays(30); 

        public bool IsConverted { get; set; } = false; 

       
        public User? User { get; set; }
        public PolicyProduct? Product { get; set; }
        public User? Agent { get; set; }
    }
}
