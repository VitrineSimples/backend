namespace Guren.DTO
{
    public class ShopDTO
    {
        public string Name { get; set; }

        public ShopDTO(string name)
        {
            Name = name;
        }

        // Construtor sem parâmetros para deserialização
        public ShopDTO() { }
    }
    public class ShopDTOOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> ProductIds { get; set; } = new List<string>();

        public ShopDTOOutput(string id, string name, List<string> productIds)
        {
            Id = id;
            Name = name;
            ProductIds = productIds;
        }

        public ShopDTOOutput() { }
    }
}