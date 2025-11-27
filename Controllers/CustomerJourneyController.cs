using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerJourneyController : ControllerBase
    {
        private readonly ICustomerJourneyService _service;

        public CustomerJourneyController(ICustomerJourneyService service)
        {
            _service = service;
        }

        [HttpPost("customer-data")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCustomerJourneyData([FromBody] CustomerJourneyRequest request)
        {
            try
            {
                var result = await _service.GetCustomerJourneyDataAsync(request.UserId, request.ProductId, request.QuoteRequest);
                
                var cacheMaxAge = !string.IsNullOrEmpty(request.UserId) ? 300 : 1800;
                Response.Headers.Add("Cache-Control", $"public, max-age={cacheMaxAge}");
                
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve customer journey data");
            }
        }

        [HttpPost("calculate-premium")]
        [AllowAnonymous]
        public async Task<ActionResult> CalculatePremiumWithContext([FromBody] PremiumCalculationRequest request)
        {
            try
            {
                var result = await _service.CalculatePremiumWithContextAsync(request.UserId, request.QuoteRequest);
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to calculate premium");
            }
        }
    }
}