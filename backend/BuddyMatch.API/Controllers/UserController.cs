using BuddyMatch.Model.Entities;
using BuddyMatch.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuddyMatch.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repository;

        public UserController(UserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Password))
                return BadRequest("Missing user data or password");

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var success = _repository.InsertUser(user);
            return success ? Ok() : StatusCode(500, "User creation failed");
        }

        [HttpPost("login")]
        public ActionResult<object> Login([FromBody] LoginRequest request)
        {
            Console.WriteLine($"🔐 Login attempt: {request.Email}");

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                Console.WriteLine("⚠️ Missing email or password.");
                return BadRequest("Email and password are required");
            }

            var user = _repository.GetByEmail(request.Email);

            if (user == null)
            {
                Console.WriteLine("❌ No user found.");
                return Unauthorized(new { message = "Invalid email or password" });
            }

            Console.WriteLine($"➡️ Stored hash: {user.Password}");
            Console.WriteLine($"➡️ Entered password: {request.Password}");

            var passwordMatch = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            Console.WriteLine($"✅ Hash match result: {passwordMatch}");

            if (passwordMatch)
            {
                var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                Console.WriteLine($"✅ Login success for: {user.Email}");
                return Ok(new
                {
                    token,
                    userId = user.Id,
                    name = user.Name,
                    email = user.Email,
                    program = user.Program
                });
            }

            Console.WriteLine("❌ Password hash mismatch.");
            return Unauthorized(new { message = "Invalid email or password" });
        }

        [HttpGet("match/{id}")]
        public ActionResult<IEnumerable<User>> GetMatches(int id)
        {
            var matches = _repository.GetMatchingUsers(id);
            return Ok(matches);
        }

        [HttpGet("profile/{id}")]
        public ActionResult<User> GetUserProfile(int id)
        {
            var user = _repository.GetUserById(id);
            return user == null ? NotFound("User not found") : Ok(user);
        }

        [HttpPut("profile/{id}")]
        public IActionResult UpdateUserProfile(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("User ID mismatch");

            var success = _repository.UpdateUser(user);
            return success ? Ok() : StatusCode(500, "Failed to update user profile");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var success = _repository.DeleteUser(id);
            return success ? Ok() : NotFound("User not found");
        }
    }
}
