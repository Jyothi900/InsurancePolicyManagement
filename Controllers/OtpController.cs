using Microsoft.AspNetCore.Mvc;
using InsurancePolicyManagement.Interfaces;

namespace InsurancePolicyManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtpController : ControllerBase
    {
        private readonly IOtpService _otpService;

        public OtpController(IOtpService otpService)
        {
            _otpService = otpService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            var result = await _otpService.SendOtpAsync(request.Email);
            if (!result)
                return BadRequest(new { message = "Email not found" });

            return Ok(new { message = "OTP sent successfully" });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var result = await _otpService.VerifyOtpAsync(request.Email, request.Otp);
            if (!result)
                return BadRequest(new { message = "Invalid or expired OTP" });

            return Ok(new { message = "OTP verified successfully" });
        }

        [HttpPost("resend")]
        public async Task<IActionResult> ResendOtp([FromBody] SendOtpRequest request)
        {
            var result = await _otpService.SendOtpAsync(request.Email);
            if (!result)
                return BadRequest(new { message = "Email not found" });

            return Ok(new { message = "OTP resent successfully" });
        }
    }

    public class SendOtpRequest
    {
        public string Email { get; set; } = string.Empty;
    }

    public class VerifyOtpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
    }
}