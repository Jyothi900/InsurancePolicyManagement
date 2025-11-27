using Microsoft.AspNetCore.Mvc;
using InsurancePolicyManagement.Interfaces;

namespace InsurancePolicyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnumController : ControllerBase
    {
        private readonly IEnumService _service;

        public EnumController(IEnumService service)
        {
            _service = service;
        }
        [HttpGet("user-roles")]
        public IActionResult GetUserRoles()
        {
            var roles = _service.GetUserRoles();
            Response.Headers.Add("Cache-Control", "public, max-age=300");
            return Ok(roles);
        }

        [HttpGet("genders")]
        public IActionResult GetGenders()
        {
            var genders = _service.GetGenders();
            Response.Headers.Add("Cache-Control", "public, max-age=300");
            return Ok(genders);
        }

        [HttpGet("kyc-statuses")]
        public IActionResult GetKYCStatuses()
        {
            var statuses = _service.GetKYCStatuses();
            return Ok(statuses);
        }

        [HttpGet("statuses")]
        public IActionResult GetStatuses()
        {
            var statuses = _service.GetStatuses();
            return Ok(statuses);
        }

        [HttpGet("policy-types")]
        public IActionResult GetPolicyTypes()
        {
            var types = _service.GetPolicyTypes();
            return Ok(types);
        }

        [HttpGet("insurance-types")]
        public IActionResult GetInsuranceTypes()
        {
            var types = _service.GetInsuranceTypes();
            return Ok(types);
        }

        [HttpGet("document-types")]
        public IActionResult GetDocumentTypes()
        {
            var types = _service.GetDocumentTypes();
            return Ok(types);
        }

        [HttpGet("payment-methods")]
        public IActionResult GetPaymentMethods()
        {
            var methods = _service.GetPaymentMethods();
            return Ok(methods);
        }

        [HttpGet("premium-frequencies")]
        public IActionResult GetPremiumFrequencies()
        {
            var frequencies = _service.GetPremiumFrequencies();
            return Ok(frequencies);
        }

        [HttpGet("all-enums")]
        public IActionResult GetAllEnums()
        {
            var allEnums = _service.GetAllEnums();
            Response.Headers.Add("Cache-Control", "public, max-age=300");
            return Ok(allEnums);
        }
    }
}