using BuddyMatch.Model.Entities;
using BuddyMatch.Model.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BuddyMatch.API.Controllers
{
    /// <summary>
    /// User APIs for Study Buddy Matching Application
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _repository;

        public UserController(UserRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var users = _repository.GetAllUsers();
            return Ok(users);
        }

        /// <summary>
        /// Create a new user profile.
        /// </summary>
        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            if (user == null)
                return BadRequest("Invalid user");

            var success = _repository.InsertUser(user);
            if (success)
                return Ok();

            return BadRequest("Failed to create user");
        }

        /// <summary>
        /// Find study buddies matching program or interests for a given user.
        /// </summary>
        [HttpGet("match/{id}")]
        public ActionResult<IEnumerable<User>> GetMatches(int id)
        {
            var matches = _repository.GetMatchingUsers(id);
            return Ok(matches);
        }
    }
}
