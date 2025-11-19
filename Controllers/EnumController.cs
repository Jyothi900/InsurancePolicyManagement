using Microsoft.AspNetCore.Mvc;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnumController : ControllerBase
    {
        [HttpGet("user-roles")]
        public IActionResult GetUserRoles()
        {
            var roles = Enum.GetValues<UserRole>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            
            // Cache for 5 minutes
            Response.Headers.Add("Cache-Control", "public, max-age=300");
            return Ok(roles);
        }

        [HttpGet("genders")]
        public IActionResult GetGenders()
        {
            var genders = Enum.GetValues<Gender>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            
            // Cache for 5 minutes
            Response.Headers.Add("Cache-Control", "public, max-age=300");
            return Ok(genders);
        }

        [HttpGet("kyc-statuses")]
        public IActionResult GetKYCStatuses()
        {
            var statuses = Enum.GetValues<KYCStatus>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            return Ok(statuses);
        }

        [HttpGet("statuses")]
        public IActionResult GetStatuses()
        {
            var statuses = Enum.GetValues<Status>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            return Ok(statuses);
        }

        [HttpGet("policy-types")]
        public IActionResult GetPolicyTypes()
        {
            var types = Enum.GetValues<PolicyType>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            return Ok(types);
        }

        [HttpGet("insurance-types")]
        public IActionResult GetInsuranceTypes()
        {
            var types = Enum.GetValues<InsuranceType>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            return Ok(types);
        }

        [HttpGet("document-types")]
        public IActionResult GetDocumentTypes()
        {
            var types = Enum.GetValues<DocumentType>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            return Ok(types);
        }

        [HttpGet("payment-methods")]
        public IActionResult GetPaymentMethods()
        {
            var methods = Enum.GetValues<PaymentMethod>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            return Ok(methods);
        }

        [HttpGet("premium-frequencies")]
        public IActionResult GetPremiumFrequencies()
        {
            var frequencies = Enum.GetValues<PremiumFrequency>()
                .Select(e => new { value = (int)e, name = e.ToString() })
                .ToList();
            return Ok(frequencies);
        }

        [HttpGet("all-enums")]
        public IActionResult GetAllEnums()
        {
            var allEnums = new
            {
                userRoles = Enum.GetValues<UserRole>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                genders = Enum.GetValues<Gender>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                kycStatuses = Enum.GetValues<KYCStatus>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                statuses = Enum.GetValues<Status>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                policyTypes = Enum.GetValues<PolicyType>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                insuranceTypes = Enum.GetValues<InsuranceType>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                documentTypes = Enum.GetValues<DocumentType>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                paymentMethods = Enum.GetValues<PaymentMethod>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList(),
                premiumFrequencies = Enum.GetValues<PremiumFrequency>()
                    .Select(e => new { value = (int)e, name = e.ToString() })
                    .ToList()
            };
            
          
            Response.Headers.Add("Cache-Control", "public, max-age=300");
            return Ok(allEnums);
        }
    }
}