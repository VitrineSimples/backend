using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guren.Model
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string? ImageURL { get; set; }

        [Required]
        [ForeignKey("Shop")]
        public string ShopId { get; set; }

        public Shop? Shop { get; set; }

        public Product(string name, decimal price, string? imageURL, Shop shop)
        {
            Name = name;
            Price = price;
            ImageURL = imageURL;
            Shop = shop;
            ShopId = shop.Id;
        }

        private Product() { }
    }
}
