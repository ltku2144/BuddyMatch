namespace BuddyMatch.Model.Entities
{
    public class UserProfile
    {
        public int Id { get; set; }   // primary key
        public int UserId { get; set; }   // foreign key to users.id
        public string Program { get; set; } = string.Empty;
        public string Interests { get; set; } = string.Empty;
        public string Availability { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; }
        
        // Navigation property
        public User? User { get; set; }
    }
}
