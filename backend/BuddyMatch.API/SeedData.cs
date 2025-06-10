using System;
using System.Threading.Tasks;
using BuddyMatch.Model.Repositories;
using BuddyMatch.Model.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace BuddyMatch.API
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();

            // Check if users exist
            var existingUsers = await userRepository.GetAllUsersAsync();
            
            if (existingUsers.Count == 0)
            {
                // Create test user with BCrypt hashed password
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword("test1234");
                
                var user = new User
                {
                    Name = "Test User", // MODIFIED: Username, FirstName, LastName to Name
                    Email = "test@example.com",
                    PasswordHash = hashedPassword
                    // Add other required properties based on your User model
                };
                
                await userRepository.CreateUserAsync(user);
                Console.WriteLine("✅ Seeded test user: test@example.com"); // MODIFIED: testuser to test@example.com
            }
        }
    }
}