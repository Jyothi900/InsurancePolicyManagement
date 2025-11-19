using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Enums;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProposalController : ControllerBase
    {
        private readonly IProposalService _service;
        private readonly IUserService _userService;

        public ProposalController(IProposalService service, IUserService userService)
        {
            _service = service;
            _userService = userService;
        }

        [HttpPost("submit")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> Submit([FromBody] ProposalSubmitRequest request)
        {
            var user = await _userService.GetByIdAsync(request.UserId);
            if (user == null)
                return NotFound("User not found");
                
            if (user.KYCStatus != KYCStatus.Verified)
                return BadRequest("KYC verification required. Please complete your KYC before submitting a proposal.");

            var proposal = await _service.SubmitAsync(request.UserId, request.ProposalData);
            return proposal == null ? BadRequest("Invalid product or data") : Ok(proposal);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> GetAll()
        {
            var proposals = await _service.GetAllAsync();
            return Ok(proposals);
        }

        [HttpGet("my-proposals")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetMyProposals([FromQuery] string userId)
        {
            var proposals = await _service.GetByUserIdAsync(userId);
            return Ok(proposals);
        }

        [HttpPost("my-proposals")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetMyProposalsSecure([FromBody] ProposalByUserRequest request)
        {
            var proposals = await _service.GetByUserIdAsync(request.UserId);
            return Ok(proposals);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetById(string id)
        {
            var proposal = await _service.GetByIdAsync(id);
            return proposal == null ? NotFound() : Ok(proposal);
        }

        [HttpGet("{id}/status")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetStatus(string id)
        {
            var status = await _service.GetStatusAsync(id);
            return status == null ? NotFound() : Ok(status);
        }

        [HttpGet("{id}/required-documents")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetRequiredDocuments(string id)
        {
            var documents = await _service.GetRequiredDocumentsAsync(id);
            return Ok(documents);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Underwriter,Agent")]
        public async Task<ActionResult> UpdateStatus(string id, [FromQuery] string status, [FromQuery] string? notes = null)
        {
            var result = await _service.UpdateStatusAsync(id, status, notes);
            return !result ? NotFound() : Ok(new { Message = "Status updated successfully" });
        }

        [HttpPost("by-id")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByIdSecure([FromBody] ProposalByIdRequest request)
        {
            var proposal = await _service.GetByIdAsync(request.ProposalId);
            return proposal == null ? NotFound() : Ok(proposal);
        }

        [HttpPost("status-check")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetStatusSecure([FromBody] ProposalByIdRequest request)
        {
            var status = await _service.GetStatusAsync(request.ProposalId);
            return status == null ? NotFound() : Ok(status);
        }

        [HttpPost("required-documents")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetRequiredDocumentsSecure([FromBody] ProposalByIdRequest request)
        {
            var documents = await _service.GetRequiredDocumentsAsync(request.ProposalId);
            return Ok(documents);
        }

        [HttpPatch("status")]
        [Authorize(Roles = "Admin,Underwriter,Agent")]
        public async Task<ActionResult> UpdateStatusSecure([FromBody] ProposalStatusUpdateRequest request)
        {
            var result = await _service.UpdateStatusAsync(request.ProposalId, request.Status, request.Notes);
            return !result ? NotFound() : Ok(new { Message = "Status updated successfully" });
        }
    }
}
