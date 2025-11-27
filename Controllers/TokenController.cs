using InsurancePolicyManagement.DTOs;
using InsurancePolicyManagement.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InsurancePolicyManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IToken _tokenService;
        private readonly IUser _userService;

        public TokenController(IConfiguration configuration, IToken tokenService, IUser userService)
        {
            _key = new SymmetricSecurityKey(UTF8Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
            _tokenService = tokenService;
            _userService = userService;
        }
      
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.GetByUsernameOrEmailAsync(loginDto.Email);

            if (user == null)
                return BadRequest(new { message = "Invalid username or password" });

            bool isPasswordValid = false;
            
            if (user.Password.StartsWith("$2a$") || user.Password.StartsWith("$2b$") || user.Password.StartsWith("$2y$"))
            {
                try
                {
                    isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    isPasswordValid = loginDto.Password == user.Password;
                }
            }
            else
            {
                isPasswordValid = loginDto.Password == user.Password;
            }

            if (!isPasswordValid)
                return BadRequest(new { message = "Invalid username or password" });

            if (!user.IsActive)
                return BadRequest(new { message = "Account is inactive" });

            var tokenString = _tokenService.GenerateToken(user);

            return Ok(new
            {
                token = tokenString,
                id = user.UserId,
                email = user.Email,
                role = user.Role
            });
        }

    }

}


