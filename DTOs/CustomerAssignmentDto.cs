namespace InsurancePolicyManagement.DTOs
{
    public class CustomerAssignmentDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? AgentId { get; set; }
        public string? AgentName { get; set; }
        public string? UnderwriterId { get; set; }
        public string? UnderwriterName { get; set; }
        public DateTime AssignedDate { get; set; }
    }
}