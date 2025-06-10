using BuddyMatch.Model.Repositories;
using BuddyMatch.Model.Entities;
using System.Threading.Tasks;

namespace BuddyMatch.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepository;
        
        public AuthService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        
        public async Task<bool> ValidateCredentialsAsync(string username, string password)
        {
            // Get the user by username
            var user = await _userRepository.GetUserByUsernameAsync(username);
            
            if (user == null)
            {
                return false;
            }
                
            // Verify password using BCrypt
            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            return isValid;
        }
    }
}