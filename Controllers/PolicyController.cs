using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _service;

        public PolicyController(IPolicyService service)
        {
            _service = service;
        }

        [HttpGet("my-policies")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetMyPolicies([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");
                    
                var policies = await _service.GetByUserIdAsync(userId);
                return Ok(policies);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve policies");
            }
        }

        [HttpPost("my-policies")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetMyPoliciesSecure([FromBody] PolicyByUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return BadRequest("User ID is required");
                    
                var policies = await _service.GetByUserIdAsync(request.UserId);
                return Ok(policies);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve policies");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Policy ID is required");
                    
                var policy = await _service.GetByIdAsync(id);
                return policy == null ? NotFound() : Ok(policy);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve policy");
            }
        }



        [HttpGet("{id}/download-pdf")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> DownloadPDF(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Policy ID is required");
                    
                var pdfBytes = await _service.GeneratePolicyPDFAsync(id);
                return pdfBytes == null ? NotFound() : File(pdfBytes, "application/pdf", "policy.pdf");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to generate PDF");
            }
        }

        [HttpGet("{id}/premium-schedule")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetPremiumSchedule(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Policy ID is required");
                    
                var schedule = await _service.GetPremiumScheduleAsync(id);
                return schedule == null ? NotFound() : Ok(schedule);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve premium schedule");
            }
        }

        [HttpPost("{id}/surrender")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Surrender(string id, [FromQuery] string userId)
        {
            var result = await _service.SurrenderAsync(id, userId);
            return !result ? BadRequest("Cannot surrender policy") : Ok(new { Message = "Policy surrendered successfully" });
        }

        [HttpPost("{id}/revive")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Revive(string id, [FromQuery] string userId)
        {
            var result = await _service.ReviveAsync(id, userId);
            return !result ? BadRequest("Cannot revive policy") : Ok(new { Message = "Policy revived successfully" });
        }

        [HttpPost("issue-from-proposal")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> IssueFromProposal([FromBody] IssueFromProposalRequest request)
        {
            var policy = await _service.IssueFromProposalAsync(request.ProposalId);
            return policy == null ? BadRequest("Cannot issue policy") : Ok(policy);
        }

        [HttpPost("by-id")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByIdSecure([FromBody] PolicyByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PolicyId))
                    return BadRequest("Policy ID is required");
                    
                var policy = await _service.GetByIdAsync(request.PolicyId);
                return policy == null ? NotFound() : Ok(policy);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve policy");
            }
        }

        [HttpPost("download-pdf")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> DownloadPDFSecure([FromBody] PolicyByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PolicyId))
                    return BadRequest("Policy ID is required");
                    
                var pdfBytes = await _service.GeneratePolicyPDFAsync(request.PolicyId);
                return pdfBytes == null ? NotFound() : File(pdfBytes, "application/pdf", "policy.pdf");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to generate PDF");
            }
        }

        [HttpPost("premium-schedule")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetPremiumScheduleSecure([FromBody] PolicyByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PolicyId))
                    return BadRequest("Policy ID is required");
                    
                var schedule = await _service.GetPremiumScheduleAsync(request.PolicyId);
                return schedule == null ? NotFound() : Ok(schedule);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve premium schedule");
            }
        }

        [HttpPost("surrender")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> SurrenderSecure([FromBody] PolicyActionRequest request)
        {
            var result = await _service.SurrenderAsync(request.PolicyId, request.UserId);
            return !result ? BadRequest("Cannot surrender policy") : Ok(new { Message = "Policy surrendered successfully" });
        }

        [HttpPost("revive")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> ReviveSecure([FromBody] PolicyActionRequest request)
        {
            var result = await _service.ReviveAsync(request.PolicyId, request.UserId);
            return !result ? BadRequest("Cannot revive policy") : Ok(new { Message = "Policy revived successfully" });
        }

        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllPoliciesForAdmin()
        {
            try
            {
                var policies = await _service.GetAllAsync();
                return Ok(policies);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve policies");
            }
        }
    }
}
