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
        public string Email { get; set; }
        public string WhatsApp { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public User Owner { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
        public List<SeasonalCampaign> SeasonalCampaigns { get; set; } = new List<SeasonalCampaign>();

        public Shop(string name, string email, string whatsApp, string userId)
        {
            Name = name;
            Email = email;
            WhatsApp = whatsApp;
            UserId = userId;
        }

        private Shop() { }
    }
}