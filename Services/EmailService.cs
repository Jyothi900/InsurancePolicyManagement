using InsurancePolicyManagement.Interfaces;
using System.Net;
using System.Net.Mail;

namespace InsurancePolicyManagement.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_config["Email:Username"], _config["Email:Password"]),
                EnableSsl = true,
            };
        }

        public async Task SendApprovalEmailAsync(string userEmail, string userName)
        {
            var smtpClient = CreateSmtpClient();
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Email:Username"]),
                Subject = "Account Approved - PolicyGuard Insurance",
                Body = $"Hi {userName},\n\nYour insurance account has been approved! You can now login to PolicyGuard.\n\nBest regards,\nPolicyGuard Team",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(userEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendOtpEmailAsync(string userEmail, string userName, string otpCode)
        {
            var smtpClient = CreateSmtpClient();
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_config["Email:Username"]),
                Subject = "Password Reset OTP - PolicyGuard Insurance",
                Body = $"Hi {userName},\n\nYour OTP for password reset is: {otpCode}\n\nThis OTP will expire in 10 minutes.\n\nIf you did not request this, please ignore this email.\n\nBest regards,\nPolicyGuard Team",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(userEmail);
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}