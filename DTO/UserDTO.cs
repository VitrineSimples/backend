namespace Guren.DTO
{
    public class UserDTOInput
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CPF { get; set; }
    }

    public class UserDTOOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }

        public UserDTOOutput(string id, string name, string email, string cpf)
        {
            this.Id = id;
            this.Name = name;
            this.Email = email;
            this.CPF = cpf;
        }
    }
}
