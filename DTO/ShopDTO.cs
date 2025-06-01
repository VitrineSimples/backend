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
        public List<string> ProductIds { get; set; } = new List<string>();

        public ShopDTOOutput(string id, string name, string ownerId, List<string> productIds)
        {
            Id = id;
            Name = name;
            OwnerId = ownerId;
            ProductIds = productIds;
        }

        public ShopDTOOutput() { }
    }
}