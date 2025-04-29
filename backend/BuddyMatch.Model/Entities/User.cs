namespace BuddyMatch.Model.Entities
{
    public class User
    {
        public int Id { get; set; }   // primary key
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Program { get; set; }
        public string Interests { get; set; }
        public string Availability { get; set; }
        public DateTime CreatedAt { get; set; }  // timestamp
    }
}
