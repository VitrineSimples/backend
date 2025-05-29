using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Guren.Model;

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
                    s.Products.Select(p => p.Id).ToList()
                )).ToList();

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
                shop.Products.Select(p => p.Id).ToList()
            );

            return Ok(shopDTO);
        }

        [HttpPost]
        public ActionResult<ShopDTOOutput> CreateShop(ShopDTO shopDTO)
        {
            bool shopExists = _dbContext.Shops
                .Any(s => s.Name.ToLower() == shopDTO.Name.ToLower());

            if (shopExists)
            {
                return Conflict("A shop with the same name already exists.");
            }

            var newShop = new Shop(shopDTO.Name);

            _dbContext.Shops.Add(newShop);
            _dbContext.SaveChanges();

            var output = new ShopDTOOutput(newShop.Id, newShop.Name, new List<string>());
            return CreatedAtAction(nameof(GetShop), new { name = newShop.Name }, output);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShop(string id, ShopDTO shopDTO)
        {
            var existingShop = _dbContext.Shops.Find(id);

            if (existingShop == null)
                return NotFound();

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
    }
}
