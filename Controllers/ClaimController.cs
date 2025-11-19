using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _service;

        public ClaimController(IClaimService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> GetAllClaims()
        {
            var claims = await _service.GetAllAsync();
            return Ok(claims);
        }

        [HttpPost("file")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> FileClaim([FromBody] ClaimFileRequest request)
        {
            var claim = await _service.FileClaimAsync(request.UserId, request.ClaimData);
            return claim == null ? BadRequest("Invalid policy or claim data") : Ok(claim);
        }

        [HttpGet("my-claims")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetMyClaims([FromQuery] string userId)
        {
            var claims = await _service.GetByUserIdAsync(userId);
            return Ok(claims);
        }

        [HttpPost("my-claims")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetMyClaimsSecure([FromBody] ClaimByUserRequest request)
        {
            var claims = await _service.GetByUserIdAsync(request.UserId);
            return Ok(claims);
        }

        [HttpGet("{claimNumber}/status")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetStatus(string claimNumber)
        {
            var status = await _service.GetStatusAsync(claimNumber);
            return status == null ? NotFound() : Ok(status);
        }

        [HttpGet("{claimNumber}/required-documents")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetRequiredDocuments(string claimNumber)
        {
            var documents = await _service.GetRequiredDocumentsAsync(claimNumber);
            return Ok(documents);
        }

        [HttpGet("{claimNumber}/timeline")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetTimeline(string claimNumber)
        {
            var timeline = await _service.GetTimelineAsync(claimNumber);
            return Ok(timeline);
        }

        [HttpPatch("{claimId}/status")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> UpdateStatus(string claimId, [FromQuery] string status, [FromQuery] string? notes = null)
        {
            var result = await _service.UpdateStatusAsync(claimId, status, notes);
            return !result ? NotFound() : Ok(new { Message = "Claim status updated successfully" });
        }

        [HttpPatch("status")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> UpdateStatusSecure([FromBody] ClaimStatusUpdateRequest request)
        {
            var result = await _service.UpdateStatusAsync(request.ClaimId, request.Status, request.Notes);
            return !result ? NotFound() : Ok(new { Message = "Claim status updated successfully" });
        }

        [HttpPost("{claimId}/approve")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> ApproveClaim(string claimId, [FromQuery] decimal approvedAmount)
        {
            var result = await _service.ApproveClaimAsync(claimId, approvedAmount);
            return !result ? NotFound() : Ok(new { Message = "Claim approved successfully" });
        }

        [HttpPost("approve")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> ApproveClaimSecure([FromBody] ClaimApproveRequest request)
        {
            var result = await _service.ApproveClaimAsync(request.ClaimId, request.ApprovedAmount);
            return !result ? NotFound() : Ok(new { Message = "Claim approved successfully" });
        }

        [HttpPost("status-check")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetStatusSecure([FromBody] ClaimStatusCheckRequest request)
        {
            var status = await _service.GetStatusAsync(request.ClaimNumber);
            return status == null ? NotFound() : Ok(status);
        }

        [HttpPost("required-documents")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetRequiredDocumentsSecure([FromBody] ClaimRequiredDocsRequest request)
        {
            var documents = await _service.GetRequiredDocumentsAsync(request.ClaimNumber);
            return Ok(documents);
        }

        [HttpPost("timeline")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetTimelineSecure([FromBody] ClaimTimelineRequest request)
        {
            var timeline = await _service.GetTimelineAsync(request.ClaimNumber);
            return Ok(timeline);
        }
    }
}
