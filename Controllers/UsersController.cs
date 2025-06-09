using Guren.Database;
using Guren.DTO;
using Guren.Model;
using Microsoft.AspNetCore.Mvc;

namespace Guren.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly PedidosDbContext dbContext;

        public UsersController(PedidosDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<UserDTOOutput>> GetUsers()
        {
            var users = dbContext.Users.Select(u =>
                new UserDTOOutput(u.Id, u.Name, u.Email, u.CPF, u.ImageUrl)
            );

            return Ok(users);
        }

        [HttpGet("{id}")]
        public ActionResult<UserDTOOutput> GetUser(string id)
        {
            User? user = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                return NotFound();
            }

            var userDTO = new UserDTOOutput(user.Id, user.Name, user.Email, user.CPF, user.ImageUrl);
            return Ok(userDTO);
        }

        [HttpPost]
        public ActionResult<User> CreateUser(UserDTOInput newUserDTO)
        {
            if (dbContext.Users.Any(user => user.CPF == newUserDTO.CPF))
            {
                return Conflict("A user with this CPF already exists");
            }

            if (dbContext.Users.Any(user => user.Email == newUserDTO.Email))
            {
                return Conflict("A user with this Email already exists");
            }

            var newUser = new User(
                newUserDTO.Name,
                newUserDTO.Email,
                newUserDTO.Password,
                newUserDTO.CPF,
                newUserDTO.ImageUrl
            );

            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(CreateUser), newUser);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, UserDTOInput userToUpdateDTO)
        {
            var existingUser = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (existingUser == null)
            {
                return NotFound();
            }

            if (dbContext.Users.Any(user => user.Id != id && user.CPF == userToUpdateDTO.CPF))
            {
                return BadRequest("A user with this CPF already exists");
            }

            existingUser.Name = userToUpdateDTO.Name;
            existingUser.Email = userToUpdateDTO.Email;
            existingUser.Password = userToUpdateDTO.Password;
            existingUser.CPF = userToUpdateDTO.CPF;
            existingUser.ImageUrl = userToUpdateDTO.ImageUrl;

            dbContext.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            dbContext.Users.Remove(user);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
