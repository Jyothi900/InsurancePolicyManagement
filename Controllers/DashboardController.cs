using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.DTOs;

namespace InsurancePolicyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _service;

        public DashboardController(IDashboardService service)
        {
            _service = service;
        }

        [HttpPost("customer-data")]
        public async Task<IActionResult> GetCustomerDashboard([FromBody] CustomerDashboardRequest request)
        {
            try
            {
                var response = await _service.GetCustomerDashboardAsync(request.UserId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public async Task<IActionResult> TestEndpoint()
        {
            try
            {
                var result = await _service.GetTestDataAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(new { 
                    message = "Dashboard controller is working but database error", 
                    timestamp = DateTime.Now,
                    databaseConnected = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet("admin-data")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetAdminDashboard()
        {
            try
            {
                var response = await _service.GetAdminDashboardAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpPost("agent-data")]
        public async Task<IActionResult> GetAgentDashboard([FromBody] AgentDashboardRequest request)
        {
            try
            {
                var response = await _service.GetAgentDashboardAsync(request.AgentId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("underwriter-data")]
        public async Task<IActionResult> GetUnderwriterDashboard([FromBody] UnderwriterDashboardRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.UnderwriterId))
                {
                    return BadRequest("UnderwriterId is required");
                }

                var response = await _service.GetUnderwriterDashboardAsync(request.UnderwriterId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message, stackTrace = ex.StackTrace });
            }
        }
    }
}