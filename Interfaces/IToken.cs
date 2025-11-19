using InsurancePolicyManagement.Models;

namespace InsurancePolicyManagement.Interfaces
{
    public interface IToken
    {
       string GenerateToken(User user);
    }
}
