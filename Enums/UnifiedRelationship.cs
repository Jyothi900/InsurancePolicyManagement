namespace InsurancePolicyManagement.Enums
{
    public enum UnifiedRelationship
    {
        // Self reference
        Self,
        
        // Immediate family
        Spouse,
        Child,
        Parent,
        Sibling,
        
        // Extended family
        Grandparent,
        Grandchild,
        Uncle,
        Aunt,
        Nephew,
        Niece,
        
        // Legal/Insurance specific
        Nominee,
        LegalHeir,
        Other
    }
}
