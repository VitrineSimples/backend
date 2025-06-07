using Guren.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; }

    public string UserId { get; set; }
    public User User { get; set; }

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    public DateTime Date { get; set; }

    public decimal TotalValue => Items.Sum(i => i.Product.Price * i.Quantity);

    public Order(List<OrderItem> items, User user, DateTime date)
    {
        Items = items;
        User = user;
        UserId = user.Id;
        Date = date;
    }

    private Order() { }
}
