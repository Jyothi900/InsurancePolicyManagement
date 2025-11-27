namespace InsurancePolicyManagement.Interfaces
{
    public interface IEnumService
    {
        object GetUserRoles();
        object GetGenders();
        object GetKYCStatuses();
        object GetStatuses();
        object GetPolicyTypes();
        object GetInsuranceTypes();
        object GetDocumentTypes();
        object GetPaymentMethods();
        object GetPremiumFrequencies();
        object GetAllEnums();
    }
}