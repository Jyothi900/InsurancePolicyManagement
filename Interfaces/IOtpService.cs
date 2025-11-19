namespace InsurancePolicyManagement.Interfaces
{
    public interface IOtpService
    {
        string GenerateOtp();
        Task<bool> SendOtpAsync(string email);
        Task<bool> VerifyOtpAsync(string email, string otp);
    }
}