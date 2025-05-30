using Guren.Database;
using Guren.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Guren.Model;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            return Ok(dbContext.Products);
        }

        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(string id)
        {
            Product? product = dbContext
                .Products
                .Include(p => p.Shop)
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public ActionResult<Product> CreateProduct(ProductDTO newProductDTO)
        {
            Shop? shop = dbContext.Shops.FirstOrDefault(s => s.Id == newProductDTO.ShopId);
            if (shop == null)
            {
                return BadRequest("Shop not found.");
            }

            Product newProduct = new Product(
                newProductDTO.Name,
                newProductDTO.Price,
                newProductDTO.ImageURL,
                shop
            );

            dbContext.Products.Add(newProduct);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetProduct), new { id = newProduct.Id }, newProduct);
        }

        [HttpPost("{id}/Upload")]
        public async Task<ActionResult<Product>> UploadImage(string id, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No image found");
            }

            Product? product = dbContext
                .Products
                .FirstOrDefault(p => p.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            string fileExtension = file.FileName.Substring(file.FileName.LastIndexOf(".") + 1);

            string folderName = "products";
            string uploadsFolderPath = Path.Combine("wwwroot", folderName);
            Directory.CreateDirectory(uploadsFolderPath);

            string fileName = $"{id}.{fileExtension}";
            string filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string serverUrl = $"{Request.Scheme}://{Request.Host}";
            string imageURL = $"{serverUrl}/{folderName}/{fileName}";

            product.ImageURL = imageURL;
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(UploadImage), product);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProduct(string id, ProductDTO productDTO)
        {
            Product? foundProduct = dbContext.Products.FirstOrDefault(p => p.Id == id);
            if (foundProduct == null)
            {
                return NotFound();
            }

            Shop? shop = dbContext.Shops.FirstOrDefault(s => s.Id == productDTO.ShopId);
            if (shop == null)
            {
                return BadRequest("Shop not found.");
            }

            foundProduct.Name = productDTO.Name;
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
            Product? foundProduct = dbContext.Products.FirstOrDefault(p => p.Id == id);
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
