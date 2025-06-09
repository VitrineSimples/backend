using Guren.Model;

namespace Guren.DTO
{
    public class ShopDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string WhatsApp { get; set; }
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
        public string Email { get; set; }
        public string WhatsApp { get; set; }
        public string OwnerId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public List<SeasonalCampaignDTO> SeasonalCampaigns { get; set; } = new List<SeasonalCampaignDTO>();

        public ShopDTOOutput(string id, string name, string email, string whatsApp, string ownerId, List<Product> products, List<SeasonalCampaignDTO> seasonalCampaigns)
        {
            Id = id;
            Name = name;
            Email = email;
            WhatsApp = whatsApp;
            OwnerId = ownerId;
            Products = products;
            SeasonalCampaigns = seasonalCampaigns;
        }

        public ShopDTOOutput() { }
    }
}