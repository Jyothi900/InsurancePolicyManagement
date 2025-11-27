using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Services
{
    public class EnumService : IEnumService
    {
        public object GetUserRoles()
        {
            return Enum.GetValues<UserRole>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetGenders()
        {
            return Enum.GetValues<Gender>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetKYCStatuses()
        {
            return Enum.GetValues<KYCStatus>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetStatuses()
        {
            return Enum.GetValues<Status>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetPolicyTypes()
        {
            return Enum.GetValues<PolicyType>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetInsuranceTypes()
        {
            return Enum.GetValues<InsuranceType>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetDocumentTypes()
        {
            return Enum.GetValues<DocumentType>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetPaymentMethods()
        {
            return Enum.GetValues<PaymentMethod>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetPremiumFrequencies()
        {
            return Enum.GetValues<PremiumFrequency>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
        }

        public object GetAllEnums()
        {
            return new
            {
                userRoles = GetUserRoles(),
                genders = GetGenders(),
                kycStatuses = GetKYCStatuses(),
                statuses = GetStatuses(),
                policyTypes = GetPolicyTypes(),
                insuranceTypes = GetInsuranceTypes(),
                documentTypes = GetDocumentTypes(),
                paymentMethods = GetPaymentMethods(),
                premiumFrequencies = GetPremiumFrequencies()
            };
        }
    }
}