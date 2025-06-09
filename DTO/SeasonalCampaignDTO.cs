using System;
using System.Collections.Generic;

namespace Guren.DTO
{
    public class SeasonalCampaignDTO
    {
        public string Id { get; set; }
        public string CampaignName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ShopId { get; set; }
        public List<string> ProductIds { get; set; }
    }

    public class SeasonalCampaignPostDTO
    {

        public string CampaignName { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ShopId { get; set; }

        public List<string> ProductIds { get; set; } = new();
    }
}