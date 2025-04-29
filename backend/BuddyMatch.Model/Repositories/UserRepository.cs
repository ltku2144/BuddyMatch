using BuddyMatch.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;

namespace BuddyMatch.Model.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users";
            var data = GetData(conn, cmd);

            while (data.Read())
            {
                users.Add(new User
                {
                    Id = (int)data["id"],
                    Name = data["name"].ToString(),
                    Email = data["email"].ToString(),
                    Password = data["password"].ToString(),
                    Program = data["program"].ToString(),
                    Interests = data["interests"].ToString(),
                    Availability = data["availability"].ToString(),
                    CreatedAt = (DateTime)data["created_at"]
                });
            }

            return users;
        }

        public bool InsertUser(User user)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO users (name, email, password, program, interests, availability)
                VALUES (@name, @email, @password, @program, @interests, @availability)";
            cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, user.Name);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, user.Email);
            cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, user.Password);
            cmd.Parameters.AddWithValue("@program", NpgsqlDbType.Text, user.Program ?? "");
            cmd.Parameters.AddWithValue("@interests", NpgsqlDbType.Text, user.Interests ?? "");
            cmd.Parameters.AddWithValue("@availability", NpgsqlDbType.Text, user.Availability ?? "");

            return InsertData(conn, cmd);
        }

        // 👇 Add GetMatchingUsers INSIDE the class, right here
        public List<User> GetMatchingUsers(int userId)
        {
            var matches = new List<User>();
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();

            // Fetch the current user
            cmd.CommandText = "SELECT * FROM users WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = GetData(conn, cmd);

            if (!reader.Read()) return matches; // No user found, return empty

            var userProgram = reader["program"].ToString();
            var userInterests = reader["interests"].ToString();

            conn.Close(); // Close the first connection

            using var conn2 = new NpgsqlConnection(ConnectionString);
            var cmd2 = conn2.CreateCommand();

            // Find matches by same program or similar interests
            cmd2.CommandText = @"
                SELECT * FROM users 
                WHERE id != @id 
                AND (program = @program OR interests ILIKE '%' || @interests || '%')
            ";

            cmd2.Parameters.AddWithValue("@id", userId);
            cmd2.Parameters.AddWithValue("@program", userProgram ?? "");
            cmd2.Parameters.AddWithValue("@interests", userInterests ?? "");

            var matchReader = GetData(conn2, cmd2);

            while (matchReader.Read())
            {
                matches.Add(new User
                {
                    Id = (int)matchReader["id"],
                    Name = matchReader["name"].ToString(),
                    Email = matchReader["email"].ToString(),
                    Password = matchReader["password"].ToString(),
                    Program = matchReader["program"].ToString(),
                    Interests = matchReader["interests"].ToString(),
                    Availability = matchReader["availability"].ToString(),
                    CreatedAt = (DateTime)matchReader["created_at"]
                });
            }

            return matches;
        }
    }
}
