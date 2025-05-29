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

        public List<Product> Products { get; set; } = new List<Product>();

        public Shop(string name)
        {
            this.Name = name;
        }

        private Shop() { }
    }
}