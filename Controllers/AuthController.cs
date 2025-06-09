using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Authorization;
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
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost("signIn")]
        public ActionResult SignIn(SignInDTO parameters)
        {
            var user = dbContext.Users
                .FirstOrDefault(u => u.Email == parameters.Email && u.Password == parameters.Password);

            if (user is null)
                return NotFound("Invalid email or password.");

            var token = GenerateToken(user);
            return Ok(new { token });
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<MeDTO> Me()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userId is null)
                return Unauthorized();

            var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            if (user is null)
                return Unauthorized();

            var shop = dbContext.Shops
                .Include(s => s.Products)
                .Include(s => s.SeasonalCampaigns)
                .FirstOrDefault(s => s.UserId == user.Id);

            var meDto = new MeDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                CPF = user.CPF,
                ImageURL = user.ImageUrl,
                Shop = shop == null ? null : new ShopDTOOutput(
                    shop.Id,
                    shop.Name,
                    shop.Email,
                    shop.WhatsApp,
                    shop.UserId,
                    shop.Products.ToList(),
                    shop.SeasonalCampaigns.Select(c => new SeasonalCampaignDTO
                    {
                        Id = c.Id,
                        CampaignName = c.CampaignName,
                        Description = c.Description,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate,
                        ShopId = c.ShopId,
                        ProductIds = c.Products.Select(p => p.Id).ToList()
                    }).ToList()
                )
            };

            return Ok(meDto);
        }
    }
}
