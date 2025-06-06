namespace Guren.DTO
{
    public class SignInDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class MeDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public ShopDTOOutput? Shop { get; set; }
    }


}
