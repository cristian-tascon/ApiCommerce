using API_Commerce.Dto;
using API_Commerce.ModelsDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Commerce.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CommerceContext _context;
        private readonly IConfiguration _config;

        public AuthController(CommerceContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        /// <summary>
        /// Generate a JWT to consume the API endpoints
        /// </summary>
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UseEmail == loginDto.Email);

            if (user == null || user.UsePassword != loginDto.Password)
            {
                return Unauthorized(new { message = "Incorrect email or password" });
            }

            var token = GenerateJwtToken(user);
            var role = user.UseRol == 1 ? "Administrador" : "Auxiliar";

            return Ok(new { token, role });
            

        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UseEmail),
                new Claim("UseId", user.UseId.ToString()),
                new Claim(ClaimTypes.Role, user.UseRol == 1 ? "Administrador" : "Auxiliar")
    };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"])),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"] 
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
