using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyProductController : ControllerBase
    {
        private readonly IPolicyProductService _service;

        public PolicyProductController(IPolicyProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var products = await _service.GetAllAsync();
                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve products");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Product ID is required");
                    
                var product = await _service.GetByIdAsync(id);
                return product == null ? NotFound() : Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve product");
            }
        }

        [HttpPost("by-id")]
        [AllowAnonymous]
        public async Task<ActionResult> GetByIdSecure([FromBody] ProductByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ProductId))
                    return BadRequest("Product ID is required");
                    
                var product = await _service.GetByIdAsync(request.ProductId);
                return product == null ? NotFound() : Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve product");
            }
        }

        [HttpGet("insurancetype/{insuranceType}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetByInsuranceType(string insuranceType)
        {
            var products = await _service.GetByCategoryAsync(insuranceType);
            return Ok(products);
        }

        [HttpPost("by-type")]
        [AllowAnonymous]
        public async Task<ActionResult> GetByInsuranceTypeSecure([FromBody] ProductByTypeRequest request)
        {
            var products = await _service.GetByCategoryAsync(request.InsuranceType);
            return Ok(products);
        }

        [HttpGet("insurancetypes")]
        [AllowAnonymous]
        public async Task<ActionResult> GetInsuranceTypes()
        {
            var insuranceTypes = await _service.GetInsuranceTypesAsync();
            return Ok(insuranceTypes);
        }

        [HttpPost("calculate-premium")]
        [AllowAnonymous]
        public async Task<ActionResult> CalculatePremium([FromBody] QuoteRequestDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                    
                var quote = await _service.CalculatePremiumAsync(dto);
                return quote == null ? BadRequest("Invalid product or eligibility criteria") : Ok(quote);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to calculate premium");
            }
        }

        [HttpGet("eligibility-check")]
        [AllowAnonymous]
        public async Task<ActionResult> CheckEligibility([FromQuery] string productId, [FromQuery] int age)
        {
            try
            {
                if (string.IsNullOrEmpty(productId) || age <= 0)
                    return BadRequest("Valid product ID and age are required");
                    
                var eligible = await _service.CheckEligibilityAsync(productId, age);
                return Ok(new { eligible, productId, age });
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to check eligibility");
            }
        }

        [HttpPost("eligibility-check")]
        [AllowAnonymous]
        public async Task<ActionResult> CheckEligibilitySecure([FromBody] ProductEligibilityRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ProductId) || request.Age <= 0)
                    return BadRequest("Valid product ID and age are required");
                    
                var eligible = await _service.CheckEligibilityAsync(request.ProductId, request.Age);
                return Ok(new { eligible, productId = request.ProductId, age = request.Age });
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to check eligibility");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromBody] PolicyProductCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var product = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(string id, [FromBody] PolicyProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpPatch("update")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateSecure([FromBody] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var dto = new PolicyProductUpdateDto
            {
                ProductName = request.ProductName,
                Description = request.Description,
                MinSumAssured = request.MinSumAssured,
                MaxSumAssured = request.MaxSumAssured,
                MinAge = request.MinAge,
                MaxAge = request.MaxAge,
                MinTerm = request.MinTerm,
                MaxTerm = request.MaxTerm
            };
            var updated = await _service.UpdateAsync(request.ProductId, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await _service.DeleteAsync(id);
            return !result ? NotFound() : Ok(new { Message = "Product deleted successfully" });
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteSecure([FromBody] ProductDeleteRequest request)
        {
            var result = await _service.DeleteAsync(request.ProductId);
            return !result ? NotFound() : Ok(new { Message = "Product deleted successfully" });
        }

        [HttpGet("products-page-data")]
        [AllowAnonymous]
        public async Task<ActionResult> GetProductsPageData()
        {
            try
            {
                var products = await _service.GetAllAsync();
                var insuranceTypes = await _service.GetInsuranceTypesAsync();
                
                var enums = new
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

                var result = new
                {
                    products,
                    insuranceTypes,
                    enums
                };

                Response.Headers.Add("Cache-Control", "public, max-age=7200");
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve products page data");
            }
        }
    }
}
