using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Guren.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly PedidosDbContext dbContext;

        public AuthController(PedidosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("id", user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        [HttpGet("me")]
        public ActionResult<User> Me()
        {
            Claim? userId = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userId is null)
            {
                throw new UnauthorizedAccessException();
            }

            User? user = dbContext.Users.Find(userId.Value);
            if (user is null)
            {
                throw new UnauthorizedAccessException();
            }

            return user;
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public ActionResult<string> SignIn(SignInDTO parameters)
        {
            User? user = dbContext
                .Users
                .FirstOrDefault(
                    u => u.Email == parameters.Email && u.Password == parameters.Password
                );

            if (user is null)
            {
                return NotFound();
            }

            return GenerateToken(user);
        }
    }
}
