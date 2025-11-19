using InsurancePolicyManagement.Models;
using InsurancePolicyManagement.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InsurancePolicyManagement.Services
{
    public class TokenService:IToken
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim("Id", user.UserId),
                new System.Security.Claims.Claim("Email", user.Email!),
                new System.Security.Claims.Claim("Role", user.Role.ToString())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: creds
            );
            string tok = new JwtSecurityTokenHandler().WriteToken(token);
            return tok;
        }
    }
}
