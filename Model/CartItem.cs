using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guren.Model
{
    public class CartItem
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [Required]
        public string ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
