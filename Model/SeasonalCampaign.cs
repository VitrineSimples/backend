using Guren.Model;
using System;
using System.Collections.Generic;

namespace Guren.Model
{
    public class SeasonalCampaign
    {
        public string Id { get; set; }

        public string CampaignName { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ShopId { get; set; }
        public Shop Shop { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}