using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Guren.Model
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CPF { get; set; }
        public Shop? Shop { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public User(string name, string email, string password, string cpf)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.CPF = cpf;
        }

        private User() { }
    }
}
