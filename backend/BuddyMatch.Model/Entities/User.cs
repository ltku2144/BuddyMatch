namespace BuddyMatch.Model.Entities
{
    public class User
    {
        public int Id { get; set; }   // primary key
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Renamed from Password
        public DateTime CreatedAt { get; set; }  // timestamp
        
        // Navigation property to UserProfile (1:1 relationship)
        public UserProfile? UserProfile { get; set; }
        
        // Convenience properties for backward compatibility with frontend
        public string Program => UserProfile?.Program ?? string.Empty;
        public string Interests => UserProfile?.Interests ?? string.Empty;
        public string Availability => UserProfile?.Availability ?? string.Empty;
    }
}
