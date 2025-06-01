using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Guren.Model
{
    public class Shop
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User Owner { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();

        public Shop(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }

        private Shop() { }
    }
}