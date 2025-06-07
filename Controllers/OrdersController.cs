using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Guren.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly PedidosDbContext dbContext;

        public OrdersController(PedidosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder(OrderDTO newOrderDTO)
        {
            if (newOrderDTO.ProductIds.Length == 0)
            {
                return BadRequest("A product list must be provided");
            }

            var groupedProducts = newOrderDTO.ProductIds
                .GroupBy(id => id)
                .ToDictionary(g => g.Key, g => g.Count());

            List<Product> products = dbContext.Products
                .Where(product => groupedProducts.Keys.Contains(product.Id))
                .ToList();

            if (products.Count != groupedProducts.Count)
            {
                return BadRequest("One or more products not found");
            }

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            string userId = userIdClaim.Value;

            var user = dbContext.Users.Find(userId);
            if (user == null)
            {
                return BadRequest("User not found");
            }

            var orderItems = products.Select(product => new OrderItem
            {
                ProductId = product.Id,
                Product = product,
                Quantity = groupedProducts[product.Id]
            }).ToList();

            Order newOrder = new Order(orderItems, user, DateTime.Now);

            dbContext.Orders.Add(newOrder);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.Id }, newOrder);
        }

        [HttpPost("fromcart")]
        public async Task<ActionResult<Order>> CreateOrderFromCart()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
                return Unauthorized();

            string userId = userIdClaim.Value;

            var user = await dbContext.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Usuário não encontrado");

            var cart = await dbContext.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.Items.Any())
                return BadRequest("Carrinho vazio");

            var orderItems = cart.Items.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                Product = item.Product,
                Quantity = item.Quantity
            }).ToList();

            var order = new Order(orderItems, user, DateTime.Now);

            dbContext.Orders.Add(order);

            dbContext.CartItems.RemoveRange(cart.Items);
            dbContext.Carts.Remove(cart);

            await dbContext.SaveChangesAsync();
            return Ok(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(string id)
        {
            Order? order = dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                        .ThenInclude(p => p.Shop)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<Order>>> GetUserOrders()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null)
                return Unauthorized();

            string userId = userIdClaim.Value;

            var orders = await dbContext.Orders
                .Where(o => o.User.Id == userId)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                .Include(o => o.User)
                .ToListAsync();

            return Ok(orders);
        }
    }
}
