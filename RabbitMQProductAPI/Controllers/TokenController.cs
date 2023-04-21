using DataAccess.Data_Access;
using DataAccess.DTO;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace RabbitMQProductAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configration;
        private readonly ApplicationContext _context;

        public TokenController(IConfiguration configration, ApplicationContext context)
        {
            _configration = configration;
            _context = context;
        }


        [HttpPost]
        public async Task<IActionResult> Post(UserDetailDTO _userData)
        {

            var userdetail = new UserDetail
            {

                Email = _userData.Email,
                Password = _userData.Password,
                UserId = _userData.Id

            };

            ArgumentException.ThrowIfNullOrEmpty(userdetail.Email);

            ArgumentException.ThrowIfNullOrEmpty(userdetail.Password);

            //        var user = await GetUser(userdetail.Email, userdetail.Password);

            if (userdetail is null)
                return BadRequest();

            //create claims details based on the user information
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, _configration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", userdetail.UserId.ToString()),
                        new Claim("DisplayName", userdetail.DisplayName),
                        new Claim("UserName", userdetail.UserName),
                        new Claim("Email", userdetail.Email)
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configration["Jwt:Issuer"],
                _configration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }




        private async Task<UserDetail> GetUser(string email, string password)
        {
            return await _context.UserDetails.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }
    }
}
