using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Guren.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly PedidosDbContext dbContext;

        public ProductsController(PedidosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductDTO>> GetProducts()
        {
            var products = dbContext.Products
                .Include(p => p.Shop)
                .Select(p => new ProductDTO
                {
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageURL = p.ImageURL,
                    ShopId = p.ShopId,
                    ShopWhatsApp = p.Shop.WhatsApp
                })
                .ToList();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public ActionResult<ProductDTO> GetProduct(string id)
        {
            var product = dbContext.Products
                .Include(p => p.Shop)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var productDTO = new ProductDTO
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageURL = product.ImageURL,
                ShopId = product.ShopId,
                ShopWhatsApp = product.Shop.WhatsApp
            };

            return Ok(productDTO);
        }

        [HttpPost]
        public ActionResult<ProductDTO> CreateProduct(ProductCreateDTO newProductDTO)
        {
            var shop = dbContext.Shops.FirstOrDefault(s => s.Id == newProductDTO.ShopId);
            if (shop == null)
            {
                return BadRequest("Shop not found.");
            }

            var newProduct = new Product(
                newProductDTO.Name,
                newProductDTO.Description,
                newProductDTO.Price,
                newProductDTO.ImageURL,
                shop
            );

            dbContext.Products.Add(newProduct);
            dbContext.SaveChanges();

            var productDTO = new ProductDTO
            {
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                ImageURL = newProduct.ImageURL,
                ShopId = newProduct.ShopId,
                ShopWhatsApp = shop.WhatsApp
            };

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, productDTO);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(string id, ProductCreateDTO productDTO)
        {
            var foundProduct = dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (foundProduct == null)
            {
                return NotFound();
            }

            var shop = dbContext.Shops.FirstOrDefault(s => s.Id == productDTO.ShopId);
            if (shop == null)
            {
                return BadRequest("Shop not found.");
            }

            foundProduct.Name = productDTO.Name;
            foundProduct.Description = productDTO.Description;
            foundProduct.Price = productDTO.Price;
            foundProduct.ImageURL = productDTO.ImageURL;
            foundProduct.Shop = shop;
            foundProduct.ShopId = shop.Id;

            dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {
            var foundProduct = dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (foundProduct == null)
            {
                return NotFound();
            }

            dbContext.Products.Remove(foundProduct);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
