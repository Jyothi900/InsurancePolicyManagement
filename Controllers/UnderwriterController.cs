using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;

namespace InsurancePolicyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Underwriter,Admin")]
    public class UnderwriterController : ControllerBase
    {
        private readonly IUnderwriterService _underwriterService;

        public UnderwriterController(IUnderwriterService underwriterService)
        {
            _underwriterService = underwriterService;
        }

        [HttpGet("dashboard/{underwriterId}")]
        public async Task<ActionResult> GetDashboard(string underwriterId)
        {
            try
            {
                if (string.IsNullOrEmpty(underwriterId))
                    return BadRequest("Underwriter ID is required");
                    
                var pendingCases = await _underwriterService.GetPendingCasesAsync(underwriterId);
                var pendingDocs = await _underwriterService.GetPendingDocumentsByUnderwriterAsync(underwriterId);
                var pendingProposals = await _underwriterService.GetPendingProposalsAsync();
                var assignedCustomers = await _underwriterService.GetAssignedCustomersAsync(underwriterId);
                
                var dashboard = new
                {
                    PendingCases = pendingCases.Count(),
                    PendingDocuments = pendingDocs.Count(),
                    PendingProposals = pendingProposals.Count(),
                    TotalWorkload = pendingCases.Count() + pendingDocs.Count() + pendingProposals.Count()
                };

                return Ok(dashboard);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve dashboard data");
            }
        }

        [HttpGet("cases/pending/{underwriterId}")]
        public async Task<ActionResult> GetPendingCases(string underwriterId)
        {
            try
            {
                if (string.IsNullOrEmpty(underwriterId))
                    return BadRequest("Underwriter ID is required");
                    
                var cases = await _underwriterService.GetPendingCasesAsync(underwriterId);
                
                var caseDtos = cases.Select(c => new UnderwritingCaseDto
                {
                    CaseId = c.CaseId,
                    UserId = c.UserId,
                    CaseType = c.CaseType,
                    Priority = c.Priority,
                    Status = c.Status,
                    AssignedAt = c.AssignedAt,
                    DaysPending = (DateTime.Now - c.AssignedAt).Days
                });

                return Ok(caseDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve pending cases");
            }
        }

        [HttpPost("cases/decision")]
       
        public async Task<ActionResult> ProcessCase([FromBody] CaseDecisionDto decision)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                    
                if (decision == null || string.IsNullOrEmpty(decision.CaseId.ToString()))
                    return BadRequest("Valid case decision is required");
                    
                var result = await _underwriterService.ProcessCaseAsync(
                    decision.CaseId.ToString(),
                    decision.Decision,
                    decision.DecisionReason,
                    decision.ApprovedSumAssured);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to process case decision");
            }
        }

        [HttpGet("documents/pending/{underwriterId}")]
        public async Task<ActionResult> GetPendingDocuments(string underwriterId)
        {
            try
            {
                if (string.IsNullOrEmpty(underwriterId))
                    return BadRequest("Underwriter ID is required");
                    
             
                var documents = await _underwriterService.GetPendingDocumentsByUnderwriterAsync(underwriterId);
                
                var docDtos = documents;

                return Ok(docDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve pending documents");
            }
        }

        [HttpPost("documents/verify")]
        public async Task<ActionResult> VerifyDocument([FromBody] DocumentDecisionDto decision)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                    
                if (decision == null || string.IsNullOrEmpty(decision.VerificationId.ToString()))
                    return BadRequest("Valid document decision is required");
                    
                var result = await _underwriterService.VerifyDocumentAsync(
                    decision.VerificationId.ToString(),
                    decision.IsVerified,
                    decision.VerificationNotes);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to verify document");
            }
        }

        [HttpPatch("kyc-status/{userId}")]
        public async Task<ActionResult> UpdateKYCStatus(string userId, [FromQuery] string status, [FromQuery] string? notes = null)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(status))
                    return BadRequest("User ID and status are required");
                    
                var result = await _underwriterService.UpdateKYCStatusAsync(userId, status, notes);
                return result ? Ok(new { Message = "KYC status updated successfully" }) : BadRequest("Failed to update KYC status");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to update KYC status");
            }
        }

        [HttpPost("dashboard")]
        public async Task<ActionResult> GetDashboardSecure([FromBody] UnderwriterByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UnderwriterId))
                    return BadRequest("Underwriter ID is required");
                    
                var pendingCases = await _underwriterService.GetPendingCasesAsync(request.UnderwriterId);
                var pendingDocs = await _underwriterService.GetPendingDocumentsByUnderwriterAsync(request.UnderwriterId);
                var pendingProposals = await _underwriterService.GetPendingProposalsAsync();
                var assignedCustomers = await _underwriterService.GetAssignedCustomersAsync(request.UnderwriterId);
                
                var dashboard = new
                {
                    PendingCases = pendingCases.Count(),
                    PendingDocuments = pendingDocs.Count(),
                    PendingProposals = pendingProposals.Count(),
                    TotalWorkload = pendingCases.Count() + pendingDocs.Count() + pendingProposals.Count()
                };

                return Ok(dashboard);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve dashboard data");
            }
        }

        [HttpPost("cases/pending")]
        public async Task<ActionResult> GetPendingCasesSecure([FromBody] UnderwriterByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UnderwriterId))
                    return BadRequest("Underwriter ID is required");
                    
                var cases = await _underwriterService.GetPendingCasesAsync(request.UnderwriterId);
                
                var caseDtos = cases.Select(c => new UnderwritingCaseDto
                {
                    CaseId = c.CaseId,
                    UserId = c.UserId,
                    CaseType = c.CaseType,
                    Priority = c.Priority,
                    Status = c.Status,
                    AssignedAt = c.AssignedAt,
                    DaysPending = (DateTime.Now - c.AssignedAt).Days
                });

                return Ok(caseDtos);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve pending cases");
            }
        }

        [HttpPost("documents/pending")]
        public async Task<ActionResult> GetPendingDocumentsSecure([FromBody] UnderwriterByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UnderwriterId))
                    return BadRequest("Underwriter ID is required");
                    
                var documents = await _underwriterService.GetPendingDocumentsByUnderwriterAsync(request.UnderwriterId);
                return Ok(documents);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve pending documents");
            }
        }

        [HttpPost("assigned-customers")]
        public async Task<ActionResult> GetAssignedCustomersSecure([FromBody] UnderwriterByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UnderwriterId))
                    return BadRequest("Underwriter ID is required");
                    
                var customers = await _underwriterService.GetAssignedCustomersAsync(request.UnderwriterId);
                return Ok(customers);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve assigned customers");
            }
        }

        [HttpPatch("kyc-status")]
        public async Task<ActionResult> UpdateKYCStatusSecure([FromBody] UnderwriterKYCUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Status))
                    return BadRequest("User ID and status are required");
                    
                var result = await _underwriterService.UpdateKYCStatusAsync(request.UserId, request.Status, request.Notes);
                return result ? Ok(new { Message = "KYC status updated successfully" }) : BadRequest("Failed to update KYC status");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to update KYC status");
            }
        }

        [HttpPatch("proposals/status")]
        public async Task<ActionResult> UpdateProposalStatusSecure([FromBody] ProposalStatusUpdateRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ProposalId))
                    return BadRequest("Proposal ID is required");
                    
                if (string.IsNullOrEmpty(request.Status))
                    return BadRequest("Status is required");
                
                var result = await _underwriterService.UpdateProposalStatusAsync(request.ProposalId, request.Status, request.Notes);
                
                if (result)
                {
                    return Ok(new { Message = "Proposal status updated successfully" });
                }
                else
                {
                    return BadRequest($"Failed to update proposal status. ProposalId: {request.ProposalId}, Status: {request.Status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateProposalStatus: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("assigned-customers/{underwriterId}")]
        public async Task<ActionResult> GetAssignedCustomers(string underwriterId)
        {
            try
            {
                if (string.IsNullOrEmpty(underwriterId))
                    return BadRequest("Underwriter ID is required");
                    
                var customers = await _underwriterService.GetAssignedCustomersAsync(underwriterId);
                return Ok(customers);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve assigned customers");
            }
        }

        [HttpGet("proposals/pending")]
        public async Task<ActionResult> GetPendingProposals()
        {
            try
            {
                var proposals = await _underwriterService.GetPendingProposalsAsync();
                return Ok(proposals);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve pending proposals");
            }
        }

        [HttpPatch("proposals/{proposalId}/status")]
        public async Task<ActionResult> UpdateProposalStatus(string proposalId, [FromQuery] string status, [FromQuery] string? notes = null)
        {
            try
            {
                if (string.IsNullOrEmpty(proposalId))
                    return BadRequest("Proposal ID is required");
                    
                if (string.IsNullOrEmpty(status))
                    return BadRequest("Status is required");
                
                // Decode URL-encoded proposal ID
                var decodedProposalId = Uri.UnescapeDataString(proposalId);
                
                // Log the request for debugging
                Console.WriteLine($"UpdateProposalStatus - Original: {proposalId}, Decoded: {decodedProposalId}, Status: {status}, Notes: {notes}");
                    
                var result = await _underwriterService.UpdateProposalStatusAsync(decodedProposalId, status, notes);
                
                if (result)
                {
                    return Ok(new { Message = "Proposal status updated successfully" });
                }
                else
                {
                    return BadRequest($"Failed to update proposal status. ProposalId: {proposalId}, Status: {status}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateProposalStatus: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
