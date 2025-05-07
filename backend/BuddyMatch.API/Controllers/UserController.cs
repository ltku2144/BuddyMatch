using BuddyMatch.Model.Entities;
using BuddyMatch.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuddyMatch.API.Controllers
{
    /// <summary>
    /// User APIs for Study Buddy Matching Application
    /// Handles user creation, retrieval, matching, update, and deletion.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repository;

        // Dependency injection of the repository
        public UserController(UserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// ✅ GET: Fetch all users
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// ✅ POST: Add a new user to the system
        /// </summary>
        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Invalid user");

            var success = _repository.InsertUser(user);
            return success ? Ok() : BadRequest("Failed to create user");
        }

        /// <summary>
        /// ✅ GET: Match user with others sharing same program or interests
        /// </summary>
        [HttpGet("match/{id}")]
        public ActionResult<IEnumerable<User>> GetMatches(int id)
        {
            var matches = _repository.GetMatchingUsers(id);
            return Ok(matches);
        }

        /// <summary>
        /// ✅ DELETE: Remove user by ID
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var success = _repository.DeleteUser(id);
            return success ? Ok() : NotFound("User not found");
        }

        /// <summary>
        /// ✅ PUT: Update user by ID
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.Id)
                return BadRequest("User ID mismatch");

            var success = _repository.UpdateUser(user);
            return success ? Ok() : BadRequest("Failed to update user");
        }
    }
}
