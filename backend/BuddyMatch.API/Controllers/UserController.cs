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
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            // Add detailed logging to debug the issue
            Console.WriteLine($"[CREATE USER] Received request data:");
            Console.WriteLine($"  Name: [{request?.Name}]");
            Console.WriteLine($"  Email: [{request?.Email}]");
            Console.WriteLine($"  PasswordHash: [{request?.PasswordHash}]");
            Console.WriteLine($"  Program: [{request?.Program}]");
            Console.WriteLine($"  Interests: [{request?.Interests}]");
            Console.WriteLine($"  Availability: [{request?.Availability}]");

            if (request == null || string.IsNullOrWhiteSpace(request.PasswordHash))
                return BadRequest("Missing user data or password");

            // Create User object with UserProfile
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = request.PasswordHash, // Plain text for development
                UserProfile = new UserProfile
                {
                    Program = request.Program ?? "",
                    Interests = request.Interests ?? "",
                    Availability = request.Availability ?? ""
                }
            };

            Console.WriteLine($"[CREATE USER] Created user object with profile:");
            Console.WriteLine($"  UserProfile.Program: [{user.UserProfile.Program}]");
            Console.WriteLine($"  UserProfile.Interests: [{user.UserProfile.Interests}]");
            Console.WriteLine($"  UserProfile.Availability: [{user.UserProfile.Availability}]");

            var success = _repository.InsertUser(user);
            Console.WriteLine($"[CREATE USER] Insert result: {success}");
            return success ? Ok() : StatusCode(500, "User creation failed");
        }

        [HttpPost("login")]
        public ActionResult<object> Login([FromBody] LoginRequest request)
        {
            // Add detailed logging
            Console.WriteLine($"[LOGIN ATTEMPT] Received Email: [{request.Email}], Received Password: [{request.Password}]");

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required");
            }

            var user = _repository.GetByEmail(request.Email);

            if (user == null)
            {
                Console.WriteLine($"[LOGIN ATTEMPT] User not found for email: [{request.Email}]");
                return Unauthorized(new { message = "Invalid email or password" });
            }

            Console.WriteLine($"[LOGIN ATTEMPT] User found. DB Email: [{user.Email}], DB PasswordHash: [{user.PasswordHash}]");

            // For development: Use plain text password comparison
            // In production, this should use BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)
            var passwordMatch = request.Password == user.PasswordHash;

            Console.WriteLine($"[LOGIN ATTEMPT] Password verification result: {passwordMatch}");

            if (passwordMatch)
            {
                var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                return Ok(new
                {
                    token,
                    userId = user.Id,
                    name = user.Name,
                    email = user.Email,
                    program = user.Program
                });
            }

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
