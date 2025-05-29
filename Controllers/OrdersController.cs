using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Guren.Model;

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

            List<Product> products = dbContext
                .Products
                .Where(
                    product => newOrderDTO.ProductIds.Contains(product.Id)
                ).ToList();

            if (products.Count != newOrderDTO.ProductIds.Length)
            {
                return BadRequest("Product not found");
            }

            Claim? userId = User.Claims.FirstOrDefault(c => c.Type == "id");
            if (userId is null)
            {
                return Unauthorized();
            }

            User? user = dbContext
                .Users
                .Find(userId.Value);

            if (user is null)
            {
                return BadRequest("User not found");
            }

            Order newOrder = new Order(products, user, DateTime.Now);

            dbContext.Orders.Add(newOrder);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(CreateOrder), newOrder);
        }

        [HttpGet("{id}")]
        public ActionResult<Order> GetOrderById(string id)
        {
            Order? order = dbContext.Orders
                .Include(o => o.User)
                .Include(o => o.Products)
                    .ThenInclude(p => p.Shop)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
    }
}