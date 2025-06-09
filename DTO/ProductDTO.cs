namespace Guren.DTO
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageURL { get; set; }
        public string ShopId { get; set; }
    }
    public class ProductDTO : ProductCreateDTO
    {
        public string ShopWhatsApp { get; set; }
    }
}
