using Guren.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Guren.Model;

namespace Guren.Model
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }

        public User User { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public DateTime Date { get; set; }

        public decimal TotalValue
        {
            get
            {
                return this.Products.Sum(p => p.Price);
            }
        }

        public Order(List<Product> products, User user, DateTime date)
        {
            this.Products = products;
            this.User = user;
            this.Date = date;
        }

        private Order() { }
    }
}