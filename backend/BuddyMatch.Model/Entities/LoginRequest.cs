using System.ComponentModel.DataAnnotations;

namespace BuddyMatch.Model.Entities
{
    /// <summary>
    /// Model for login requests
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// Email address for authentication
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        /// <summary>
        /// Password for authentication
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
