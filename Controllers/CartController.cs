using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Guren.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly PedidosDbContext _context;

        public CartController(PedidosDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<CartDTO>> GetCart(string userId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return NotFound();
            }

            var cartDto = new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.Items.Select(i => new CartItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    ProductPrice = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()
            };

            return Ok(cartDto);
        }

        [HttpPost("{userId}/add")]
        public async Task<ActionResult> AddToCart(string userId, [FromBody] AddCartItemDTO dto)
        {
            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                return NotFound("Produto não encontrado.");

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId
                };
                _context.Carts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    Id = Guid.NewGuid().ToString(),
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity,
                    CartId = cart.Id
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{userId}/remove/{productId}")]
        public async Task<ActionResult> RemoveFromCart(string userId, string productId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Carrinho não encontrado.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return NotFound("Produto não está no carrinho.");

            cart.Items.Remove(item);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
