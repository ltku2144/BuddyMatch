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
                Console.WriteLine($"[AUTH] User not found: {username}");
                return false;
            }
                
            // For development: Use plain text password comparison
            // In production, this should use BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
            bool isValid = password == user.PasswordHash;
            
            Console.WriteLine($"[AUTH] Password validation for {username}: {isValid}");
            return isValid;
        }
    }
}