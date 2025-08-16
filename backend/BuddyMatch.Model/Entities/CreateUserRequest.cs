namespace BuddyMatch.Model.Entities
{
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        
        // Profile fields at the same level
        public string Program { get; set; } = string.Empty;
        public string Interests { get; set; } = string.Empty;
        public string Availability { get; set; } = string.Empty;
    }
}
