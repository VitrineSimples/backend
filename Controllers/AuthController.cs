using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        [HttpGet("me")]
        public ActionResult<MeDTO> Me()
        {
            Claim? userId = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userId is null)
            {
                return Unauthorized();
            }

            string id = userId.Value;

            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);
            if (user is null)
            {
                return Unauthorized();
            }

            var shop = dbContext.Shops
                .Include(s => s.Products)
                .FirstOrDefault(s => s.UserId == id);

            var userDto = new MeDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CPF = user.CPF,
                Shop = shop is null ? null : new ShopDTOOutput(
                    shop.Id,
                    shop.Name,
                    shop.UserId,
                    shop.Products.ToList()
                )
            };

            return Ok(userDto);
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

            return Ok(new { token = GenerateToken(user) });
        }
    }
}
