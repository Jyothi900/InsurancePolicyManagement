namespace InsurancePolicyManagement.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpEmailAsync(string userEmail, string userName, string otpCode);
        Task SendApprovalEmailAsync(string userEmail, string userName);
    }
}