using System.Collections.Generic;

namespace Guren.DTO
{
    public class CartDTO
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public List<CartItemDTO> Items { get; set; } = new();
    }
}
