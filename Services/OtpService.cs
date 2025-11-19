using InsurancePolicyManagement.Interfaces;
using InsurancePolicyManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace InsurancePolicyManagement.Services
{
    public class OtpService : IOtpService
    {
        private readonly InsuranceManagementContext _context;
        private readonly IEmailService _emailService;

        public OtpService(InsuranceManagementContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public string GenerateOtp()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        public async Task<bool> SendOtpAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return false;

            var otp = GenerateOtp();
            user.OtpCode = otp;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();
            await _emailService.SendOtpEmailAsync(email, user.FullName ?? "User", otp);
            return true;
        }

        public async Task<bool> VerifyOtpAsync(string email, string otp)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || user.OtpCode != otp || user.OtpExpiry < DateTime.UtcNow)
                return false;

            user.OtpCode = null;
            user.OtpExpiry = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}