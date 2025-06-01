using Guren.Model;

namespace Guren.DTO
{
    public class ShopDTO
    {
        public string Name { get; set; }
        public string UserId { get; set; }

        public ShopDTO(string name, string userId)
        {
            Name = name;
            UserId = userId;
        }

        public ShopDTO() { }
    }

    public class ShopDTOOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();

        public ShopDTOOutput(string id, string name, string ownerId, List<Product> products)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
            Products = products;
        }

        public ShopDTOOutput() { }
    }
}