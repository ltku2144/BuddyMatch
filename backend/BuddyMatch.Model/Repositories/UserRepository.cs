using BuddyMatch.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;

namespace BuddyMatch.Model.Repositories
{
    public class UserRepository : BaseRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users";
            var reader = GetData(conn, cmd);

            while (reader.Read())
            {
                users.Add(ReadUser(reader));
            }
            return users;
        }

        public bool InsertUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password ?? string.Empty);

            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO users (name, email, password, program, interests, availability)
                VALUES (@name, @email, @password, @program, @interests, @availability)";

            cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, user.Name ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, user.Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, user.Password);
            cmd.Parameters.AddWithValue("@program", NpgsqlDbType.Text, user.Program ?? string.Empty);
            cmd.Parameters.AddWithValue("@interests", NpgsqlDbType.Text, user.Interests ?? string.Empty);
            cmd.Parameters.AddWithValue("@availability", NpgsqlDbType.Text, user.Availability ?? string.Empty);

            return InsertData(conn, cmd);
        }

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
                matches.Add(ReadUser(matchReader));
            }

            return matches;
        }

        public bool DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM users WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            return InsertData(conn, cmd);
        }

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

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                return false;
            }
        }

        public User? GetByEmail(string email)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE email = @Email";
            cmd.Parameters.AddWithValue("@Email", email);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadUser(reader) : null;
        }

        public User? GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM users WHERE id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadUser(reader) : null;
        }

        private User ReadUser(NpgsqlDataReader reader)
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
    }
}
