namespace BuddyMatch.Model.Entities
{
    public class User
    {
        public int Id { get; set; }   // primary key
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public string Interests { get; set; } = string.Empty;
        public string Availability { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }  // timestamp
    }
}
