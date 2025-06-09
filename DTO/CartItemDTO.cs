namespace Guren.DTO
{
    public class CartItemDTO
    {
        public string Id { get; set; } = null!;
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        public string ShopId { get; set; }
        public string ShopWhatsApp { get; set; } = null!;
        public int Quantity { get; set; }
    }
}