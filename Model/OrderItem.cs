using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guren.Model
{
    public class OrderItem
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string ProductId { get; set; }
        public Product Product { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string OrderId { get; set; }
        public Order Order { get; set; }
    }
}
