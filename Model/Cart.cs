using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guren.Model
{
    public class Cart
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
