using BuddyMatch.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;

namespace BuddyMatch.Model.Repositories
{
    /// <summary>
    /// Repository class for interacting with the PostgreSQL 'users' table.
    /// Encapsulates all CRUD logic (Create, Read, Update, Delete) for user data.
    /// </summary>
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) {}

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        public List<User> GetAllUsers()
        {
            Console.WriteLine("✔ Fetched users from DB");
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
                    Name = data["name"]?.ToString() ?? string.Empty,
                    Email = data["email"]?.ToString() ?? string.Empty,
                    Password = data["password"]?.ToString() ?? string.Empty,
                    Program = data["program"]?.ToString() ?? string.Empty,
                    Interests = data["interests"]?.ToString() ?? string.Empty,
                    Availability = data["availability"]?.ToString() ?? string.Empty,
                    CreatedAt = (DateTime)data["created_at"]
                });
            }
            return users;
        }

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        public bool InsertUser(User user)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO users (name, email, password, program, interests, availability)
                VALUES (@name, @email, @password, @program, @interests, @availability)";

            cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, user.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, user.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, user.Password ?? string.Empty);
            cmd.Parameters.AddWithValue("@program", NpgsqlDbType.Text, user.Program ?? string.Empty);
            cmd.Parameters.AddWithValue("@interests", NpgsqlDbType.Text, user.Interests ?? string.Empty);
            cmd.Parameters.AddWithValue("@availability", NpgsqlDbType.Text, user.Availability ?? string.Empty);

            return InsertData(conn, cmd);
        }

        /// <summary>
        /// Finds users with matching programs or interests.
        /// </summary>
        public List<User> GetMatchingUsers(int userId)
        {
            var matches = new List<User>();
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = GetData(conn, cmd);

            if (!reader.Read()) return matches;

            var userProgram = reader["program"]?.ToString() ?? string.Empty;
            var userInterests = reader["interests"]?.ToString() ?? string.Empty;
            conn.Close();

            using var conn2 = new NpgsqlConnection(ConnectionString);
            var cmd2 = conn2.CreateCommand();
            cmd2.CommandText = @"
                SELECT * FROM users
                WHERE id != @id
                AND (program = @program OR interests ILIKE '%' || @interests || '%')";

            cmd2.Parameters.AddWithValue("@id", userId);
            cmd2.Parameters.AddWithValue("@program", userProgram);
            cmd2.Parameters.AddWithValue("@interests", userInterests);

            var matchReader = GetData(conn2, cmd2);
            while (matchReader.Read())
            {
                matches.Add(new User
                {
                    Id = (int)matchReader["id"],
                    Name = matchReader["name"]?.ToString() ?? string.Empty,
                    Email = matchReader["email"]?.ToString() ?? string.Empty,
                    Password = matchReader["password"]?.ToString() ?? string.Empty,
                    Program = matchReader["program"]?.ToString() ?? string.Empty,
                    Interests = matchReader["interests"]?.ToString() ?? string.Empty,
                    Availability = matchReader["availability"]?.ToString() ?? string.Empty,
                    CreatedAt = (DateTime)matchReader["created_at"]
                });
            }

            return matches;
        }

        /// <summary>
        /// Deletes a user from the database by ID.
        /// </summary>
        public bool DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM users WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            return InsertData(conn, cmd);
        }

        /// <summary>
        /// Updates an existing user record.
        /// </summary>
        public bool UpdateUser(User user)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                UPDATE users SET
                    name = @name,
                    email = @email,
                    program = @program,
                    interests = @interests,
                    availability = @availability
                WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", user.Id);
            cmd.Parameters.AddWithValue("@name", user.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", user.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@program", user.Program ?? string.Empty);
            cmd.Parameters.AddWithValue("@interests", user.Interests ?? string.Empty);
            cmd.Parameters.AddWithValue("@availability", user.Availability ?? string.Empty);

            return InsertData(conn, cmd);
        }
    }
}
