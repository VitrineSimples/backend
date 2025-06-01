using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guren.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopsController : ControllerBase
    {
        private readonly PedidosDbContext _dbContext;

        public ShopsController(PedidosDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShopDTOOutput>> GetShops()
        {
            var shops = _dbContext.Shops
                .Include(s => s.Products)
                .Select(s => new ShopDTOOutput(
                    s.Id,
                    s.Name,
                    s.UserId,
                    s.Products.ToList()
                ))
                .ToList();

            return Ok(shops);
        }

        [HttpGet("{name}")]
        public ActionResult<ShopDTOOutput> GetShop(string name)
        {
            var shop = _dbContext.Shops
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Name == name);

            if (shop == null)
                return NotFound();

            var shopDTO = new ShopDTOOutput(
                shop.Id,
                shop.Name,
                shop.UserId,
                shop.Products.ToList()
            );

            return Ok(shopDTO);
        }

        [HttpPost]
        public ActionResult<ShopDTOOutput> CreateShop(ShopDTO shopDTO)
        {
            var owner = _dbContext.Users.FirstOrDefault(u => u.Id == shopDTO.UserId);
            if (owner == null)
            {
                return BadRequest("User (owner) not found.");
            }

            if (_dbContext.Shops.Any(s => s.UserId == shopDTO.UserId))
            {
                return Conflict("This user already owns a shop.");
            }

            if (_dbContext.Shops.Any(s => s.Name.ToLower() == shopDTO.Name.ToLower()))
            {
                return Conflict("A shop with the same name already exists.");
            }

            var newShop = new Shop(shopDTO.Name, shopDTO.UserId);

            _dbContext.Shops.Add(newShop);
            _dbContext.SaveChanges();

            var output = new ShopDTOOutput(
                newShop.Id,
                newShop.Name,
                newShop.UserId,
                new List<Product>()
            );

            return CreatedAtAction(nameof(GetShop), new { name = newShop.Name }, output);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShop(string id, ShopDTO shopDTO)
        {
            var existingShop = _dbContext.Shops.FirstOrDefault(s => s.Id == id);

            if (existingShop == null)
                return NotFound();

            bool nameExists = _dbContext.Shops
                .Any(s => s.Id != id && s.Name.ToLower() == shopDTO.Name.ToLower());

            if (nameExists)
            {
                return Conflict("A shop with the same name already exists.");
            }

            if (shopDTO.UserId != null && existingShop.UserId != shopDTO.UserId)
            {
                return BadRequest("Changing the shop owner is not allowed.");
            }

            existingShop.Name = shopDTO.Name;
            _dbContext.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteShop(string id)
        {
            var existingShop = _dbContext.Shops
                .Include(s => s.Products)
                .FirstOrDefault(s => s.Id == id);

            if (existingShop == null)
                return NotFound();

            _dbContext.Shops.Remove(existingShop);
            _dbContext.SaveChanges();

            return NoContent();
        }

        [HttpGet("user/{userId}")]
        public ActionResult<ShopDTOOutput> GetShopByUserId(string userId)
        {
            var shop = _dbContext.Shops
                .Include(s => s.Products)
                .FirstOrDefault(s => s.UserId == userId);

            if (shop == null)
                return NotFound();

            var shopDTO = new ShopDTOOutput(
                shop.Id,
                shop.Name,
                shop.UserId,
                shop.Products.ToList()
            );

            return Ok(shopDTO);
        }
    }
}
