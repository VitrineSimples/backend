using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Guren.Database;
using Guren.DTO;
using Guren.Model;
using System.Linq;

namespace Guren.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeasonalCampaignController : ControllerBase
    {
        private readonly PedidosDbContext _context;

        public SeasonalCampaignController(PedidosDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeasonalCampaign>>> GetCampaigns()
        {
            return await _context.SeasonalCampaigns
                .Include(c => c.Products)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SeasonalCampaign>> GetCampaign(string id)
        {
            var campaign = await _context.SeasonalCampaigns
                .Include(c => c.Products)
                .Include(c => c.Shop)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
                return NotFound();

            return campaign;
        }

        [HttpPost]
        public async Task<ActionResult<SeasonalCampaign>> CreateCampaign(SeasonalCampaignPostDTO dto)
        {
            var shop = await _context.Shops
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == dto.ShopId);

            if (shop == null)
                return BadRequest("Shop not found.");

            var products = shop.Products.Where(p => dto.ProductIds.Contains(p.Id)).ToList();

            if (products.Count != dto.ProductIds.Count)
                return BadRequest("Some products do not belong to the selected shop.");

            var campaign = new SeasonalCampaign
            {
                Id = Guid.NewGuid().ToString(),
                CampaignName = dto.CampaignName,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                ShopId = dto.ShopId,
                Products = products
            };

            _context.SeasonalCampaigns.Add(campaign);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCampaign), new { id = campaign.Id }, campaign);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCampaign(string id, SeasonalCampaignDTO dto)
        {
            var campaign = await _context.SeasonalCampaigns
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (campaign == null)
                return NotFound();

            if (campaign.ShopId != dto.ShopId)
                return BadRequest("Cannot change the shop of the campaign.");

            campaign.CampaignName = dto.CampaignName;
            campaign.Description = dto.Description;
            campaign.StartDate = dto.StartDate;
            campaign.EndDate = dto.EndDate;

            var shopProducts = await _context.Products
                .Where(p => p.ShopId == dto.ShopId && dto.ProductIds.Contains(p.Id))
                .ToListAsync();

            if (shopProducts.Count != dto.ProductIds.Count)
                return BadRequest("Some products do not belong to the selected shop.");

            campaign.Products = shopProducts;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign(string id)
        {
            var campaign = await _context.SeasonalCampaigns.FindAsync(id);

            if (campaign == null)
                return NotFound();

            _context.SeasonalCampaigns.Remove(campaign);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("shop/{shopId}")]
        public async Task<ActionResult<IEnumerable<SeasonalCampaign>>> GetCampaignsByShop(string shopId)
        {
            var shopExists = await _context.Shops.AnyAsync(s => s.Id == shopId);
            if (!shopExists)
                return NotFound("Shop not found.");

            var campaigns = await _context.SeasonalCampaigns
                .Where(c => c.ShopId == shopId)
                .Include(c => c.Products)
                .ToListAsync();

            return Ok(campaigns);
        }
    }
}