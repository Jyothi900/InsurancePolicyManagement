namespace InsurancePolicyManagement.Enums
{
    public enum Status
    {
        // General statuses
        Active,
        Inactive,
        Pending,
        Approved,
        Rejected,
        Cancelled,
        Expired,
        
        // Policy specific
        Lapsed,
        Surrendered,
        Matured,
        Claimed,
        
        // Claim specific
        Filed,
        UnderInvestigation,
        SurveyorAssigned,
        Settled,
        
        // Payment specific
        Success,
        Failed,
        Refunded,
        
        // Document specific
        Uploaded,
        Verified,
        
        // Proposal specific
        Applied,
        UnderReview,
        Issued
    }
}
