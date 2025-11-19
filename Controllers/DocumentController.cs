using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _service;

        public DocumentController(IDocumentService service)
        {
            _service = service;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> Upload([FromForm] DocumentUploadDto dto)
        {
            var document = await _service.UploadAsync(dto);
            return document == null ? BadRequest("Invalid file or data") : Ok(document);
        }

        [HttpPost("upload-kyc")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> UploadKYC([FromForm] KYCDocumentUploadDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UploadKYCDocumentsAsync(dto);
            return result == null ? BadRequest("Failed to upload KYC documents") : Ok(result);
        }

        [HttpGet("my-documents")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetMyDocuments([FromQuery] string userId)
        {
            var documents = await _service.GetByUserIdAsync(userId);
            return Ok(documents);
        }

        [HttpPost("my-documents")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetMyDocumentsSecure([FromBody] DocumentByUserRequest request)
        {
            var documents = await _service.GetByUserIdAsync(request.UserId);
            return Ok(documents);
        }

        [HttpGet("proposal/{proposalId}")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByProposal(string proposalId)
        {
            var documents = await _service.GetByProposalIdAsync(proposalId);
            return Ok(documents);
        }

        [HttpGet("policy/{policyId}")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByPolicy(string policyId)
        {
            var documents = await _service.GetByPolicyIdAsync(policyId);
            return Ok(documents);
        }

        [HttpGet("claim/{claimId}")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByClaim(string claimId)
        {
            var documents = await _service.GetByClaimIdAsync(claimId);
            return Ok(documents);
        }

        [HttpGet("pending-verification")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> GetPendingVerification()
        {
            var documents = await _service.GetPendingVerificationAsync();
            return Ok(documents);
        }

        [HttpGet("all-for-verification")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> GetAllForVerification()
        {
            var documents = await _service.GetAllForVerificationAsync();
            return Ok(documents);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetById(string id)
        {
            var document = await _service.GetByIdAsync(id);
            return document == null ? NotFound() : Ok(document);
        }

        [HttpGet("{id}/download")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> Download(string id)
        {
            var fileBytes = await _service.DownloadAsync(id);
            if (fileBytes == null) return NotFound();

            var document = await _service.GetByIdAsync(id);
            var contentType = GetContentType(document.FileName);
            return File(fileBytes, contentType, document.FileName);
        }

        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream"
            };
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> Delete(string id, [FromQuery] string userId)
        {
            var result = await _service.DeleteAsync(id, userId);
            return !result ? BadRequest("Cannot delete document") : Ok(new { Message = "Document deleted successfully" });
        }

        [HttpPost("by-proposal")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByProposalSecure([FromBody] DocumentByProposalRequest request)
        {
            var documents = await _service.GetByProposalIdAsync(request.ProposalId);
            return Ok(documents);
        }

        [HttpPost("by-policy")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByPolicySecure([FromBody] DocumentByPolicyRequest request)
        {
            var documents = await _service.GetByPolicyIdAsync(request.PolicyId);
            return Ok(documents);
        }

        [HttpPost("by-claim")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByClaimSecure([FromBody] DocumentByClaimRequest request)
        {
            var documents = await _service.GetByClaimIdAsync(request.ClaimId);
            return Ok(documents);
        }

        [HttpPost("by-id")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> GetByIdSecure([FromBody] DocumentByIdRequest request)
        {
            var document = await _service.GetByIdAsync(request.DocumentId);
            return document == null ? NotFound() : Ok(document);
        }

        [HttpPost("download")]
        [Authorize(Roles = "Customer,Agent,Admin,Underwriter")]
        public async Task<ActionResult> DownloadSecure([FromBody] DocumentByIdRequest request)
        {
            var fileBytes = await _service.DownloadAsync(request.DocumentId);
            if (fileBytes == null) return NotFound();

            var document = await _service.GetByIdAsync(request.DocumentId);
            var contentType = GetContentType(document.FileName);
            return File(fileBytes, contentType, document.FileName);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> DeleteSecure([FromBody] DocumentDeleteRequest request)
        {
            var result = await _service.DeleteAsync(request.DocumentId, request.UserId);
            return !result ? BadRequest("Cannot delete document") : Ok(new { Message = "Document deleted successfully" });
        }

        [HttpPost("verify")]
        [Authorize(Roles = "Admin,Underwriter")]
        public async Task<ActionResult> VerifyDocument([FromBody] SimpleDocumentVerificationDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                    
                var result = await _service.VerifyDocumentAsync(dto.DocumentId, dto.IsVerified, dto.VerificationNotes);
                return result ? Ok(new { Message = "Document verification updated successfully" }) : BadRequest("Failed to verify document");
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to verify document");
            }
        }



    }
}
