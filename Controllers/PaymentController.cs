using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using AutoMapper;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService service)
        {
            _service = service;
        }
        [HttpGet("history")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetHistory([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");
                
                var history = await _service.GetHistoryAsync(userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve payment history: {ex.Message}");
            }
        }

        [HttpGet("due-premiums")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetDuePremiums([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");
                    
                var duePremiums = await _service.GetDuePremiumsAsync(userId);
                return Ok(duePremiums);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve due premiums");
            }
        }

        [HttpGet("pending-payments")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetPendingPayments([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");
                    
                var pendingPayments = await _service.GetPendingPaymentsAsync(userId);
                return Ok(pendingPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve pending payments: {ex.Message}");
            }
        }

        [HttpPost("pay-premium")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> PayPremium([FromBody] PayPremiumRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PolicyId) || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.PaymentMethod))
                    return BadRequest("Policy ID, User ID, and Payment Method are required");
                    
                var payment = await _service.PayPremiumAsync(request.PolicyId, request.UserId, request.PaymentMethod);
                return payment == null ? BadRequest("Cannot process premium payment") : Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to process payment: {ex.Message}");
            }
        }

        [HttpPost("pay-proposal")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> PayProposal([FromBody] PayProposalRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ProposalId) || string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.PaymentMethod))
                    return BadRequest("Proposal ID, User ID, and Payment Method are required");
                    
                var payment = await _service.PayProposalAsync(request.ProposalId, request.UserId, request.PaymentMethod);
                return payment == null ? BadRequest("Cannot process proposal payment") : Ok(payment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to process payment: {ex.Message}");
            }
        }

        [HttpPost("history")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetHistorySecure([FromBody] PaymentByUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return BadRequest("User ID is required");
                
                var history = await _service.GetHistoryAsync(request.UserId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve payment history: {ex.Message}");
            }
        }

        [HttpPost("due-premiums")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetDuePremiumsSecure([FromBody] PaymentByUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return BadRequest("User ID is required");
                    
                var duePremiums = await _service.GetDuePremiumsAsync(request.UserId);
                return Ok(duePremiums);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve due premiums");
            }
        }

        [HttpPost("pending-payments")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetPendingPaymentsSecure([FromBody] PaymentByUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return BadRequest("User ID is required");
                    
                var pendingPayments = await _service.GetPendingPaymentsAsync(request.UserId);
                return Ok(pendingPayments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve pending payments: {ex.Message}");
            }
        }

        [HttpPost("issued-proposals")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetIssuedProposalsSecure([FromBody] PaymentByUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.UserId))
                    return BadRequest("User ID is required");
                    
                var proposals = await _service.GetIssuedProposalsAsync(request.UserId);
                return Ok(proposals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve issued proposals: {ex.Message}");
            }
        }

        [HttpPost("receipt")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetReceiptSecure([FromBody] PaymentReceiptRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.TransactionId))
                    return BadRequest("Transaction ID is required");
                    
                var receipt = await _service.GetReceiptAsync(request.TransactionId);
                return receipt == null ? NotFound() : Ok(receipt);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve receipt");
            }
        }

        [HttpPost("by-id")]
        [Authorize(Roles = "Agent,Admin")]
        public async Task<ActionResult> GetByIdSecure([FromBody] PaymentByIdRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.PaymentId))
                    return BadRequest("Payment ID is required");
                    
                var payment = await _service.GetByIdAsync(request.PaymentId);
                return payment == null ? NotFound() : Ok(payment);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve payment");
            }
        }

        [HttpGet("issued-proposals")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetIssuedProposals([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("User ID is required");
                    
                var proposals = await _service.GetIssuedProposalsAsync(userId);
                return Ok(proposals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve issued proposals: {ex.Message}");
            }
        }

        [HttpGet("receipt/{transactionId}")]
        [Authorize(Roles = "Customer,Agent,Admin")]
        public async Task<ActionResult> GetReceipt(string transactionId)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionId))
                    return BadRequest("Transaction ID is required");
                    
                var receipt = await _service.GetReceiptAsync(transactionId);
                return receipt == null ? NotFound() : Ok(receipt);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve receipt");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Agent,Admin")]
        public async Task<ActionResult> GetById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Payment ID is required");
                    
                var payment = await _service.GetByIdAsync(id);
                return payment == null ? NotFound() : Ok(payment);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve payment");
            }
        }
    }
}
