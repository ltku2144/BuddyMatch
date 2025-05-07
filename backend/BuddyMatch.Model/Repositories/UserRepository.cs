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
        /// Updates an existing user record in the database.
        /// </summary>
        /// <param name="user">The user object containing updated information.</param>
        /// <returns>True if the update was successful, otherwise false.</returns>
        public bool UpdateUser(User user)
        {
            try
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE users
                    SET name = @Name,
                        email = @Email,
                        program = @Program,
                        interests = @Interests,
                        availability = @Availability
                    WHERE id = @Id";
                cmd.Parameters.AddWithValue("@Name", user.Name);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@Program", user.Program ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Interests", user.Interests ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Availability", user.Availability ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", user.Id);

                Console.WriteLine($"Executing SQL: {cmd.CommandText}");
                Console.WriteLine($"Parameters: Name={user.Name}, Email={user.Email}, Program={user.Program}, Interests={user.Interests}, Availability={user.Availability}, Id={user.Id}");

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The user object if found, otherwise null.</returns>
        public User GetByEmail(string email)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE email = @Email";
            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    Program = reader.GetString(reader.GetOrdinal("program")),
                    Interests = reader.GetString(reader.GetOrdinal("interests")),
                    Availability = reader.GetString(reader.GetOrdinal("availability")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                };
            }

            return null;
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user object if found, otherwise null.</returns>
        public User? GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    Program = reader.GetString(reader.GetOrdinal("program")),
                    Interests = reader.GetString(reader.GetOrdinal("interests")),
                    Availability = reader.GetString(reader.GetOrdinal("availability")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                };
            }

            return null;
        }
    }
}
