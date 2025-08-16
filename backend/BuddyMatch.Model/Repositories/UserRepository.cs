using BuddyMatch.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuddyMatch.Model.Repositories
{
    public class UserRepository : BaseRepository
    {
        private readonly UserProfileRepository _userProfileRepository;

        public UserRepository(IConfiguration configuration) : base(configuration) 
        {
            _userProfileRepository = new UserProfileRepository(configuration);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            // This remains synchronous but wrapped in a Task for signature compatibility
            return await Task.FromResult(GetAllUsers());
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT u.id, u.name, u.email, u.password, u.created_at,
                       p.id as profile_id, p.program, p.interests, p.availability, p.updated_at
                FROM public.users u
                LEFT JOIN public.user_profiles p ON u.id = p.user_id";
            var reader = GetData(conn, cmd);

            while (reader.Read())
            {
                users.Add(ReadUserWithProfile(reader));
            }
            return users;
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            // This remains synchronous but wrapped in a Task for signature compatibility
            return await Task.FromResult(InsertUser(user));
        }

        public bool InsertUser(User user)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            conn.Open();
            using var transaction = conn.BeginTransaction();
            
            try
            {
                // Insert into users table first
                var userCmd = conn.CreateCommand();
                userCmd.Transaction = transaction;
                userCmd.CommandText = @"
                    INSERT INTO public.users (name, email, password)
                    VALUES (@name, @email, @password)
                    RETURNING id";

                userCmd.Parameters.AddWithValue("@name", NpgsqlDbType.Text, user.Name ?? string.Empty);
                userCmd.Parameters.AddWithValue("@email", NpgsqlDbType.Text, user.Email ?? string.Empty);
                userCmd.Parameters.AddWithValue("@password", NpgsqlDbType.Text, user.PasswordHash);

                var newUserId = (int)(userCmd.ExecuteScalar() ?? 0);
                
                if (newUserId > 0 && user.UserProfile != null)
                {
                    // Insert into user_profiles table
                    var profileCmd = conn.CreateCommand();
                    profileCmd.Transaction = transaction;
                    profileCmd.CommandText = @"
                        INSERT INTO public.user_profiles (user_id, program, interests, availability)
                        VALUES (@user_id, @program, @interests, @availability)";

                    profileCmd.Parameters.AddWithValue("@user_id", newUserId);
                    profileCmd.Parameters.AddWithValue("@program", NpgsqlDbType.Text, user.UserProfile.Program ?? string.Empty);
                    profileCmd.Parameters.AddWithValue("@interests", NpgsqlDbType.Text, user.UserProfile.Interests ?? string.Empty);
                    profileCmd.Parameters.AddWithValue("@availability", NpgsqlDbType.Text, user.UserProfile.Availability ?? string.Empty);

                    profileCmd.ExecuteNonQuery();
                }
                
                transaction.Commit();
                return newUserId > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Error in InsertUser: {ex.Message}");
                return false;
            }
        }

        public List<User> GetMatchingUsers(int userId)
        {
            var matches = new List<User>();

            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT u.id, u.name, u.email, u.password, u.created_at,
                       p.id as profile_id, p.program, p.interests, p.availability, p.updated_at
                FROM public.users u
                LEFT JOIN public.user_profiles p ON u.id = p.user_id
                WHERE u.id = @id";
            cmd.Parameters.AddWithValue("@id", userId);
            var reader = GetData(conn, cmd);

            if (!reader.Read()) return matches;

            var userProgram = reader["program"]?.ToString() ?? string.Empty;
            var userInterests = reader["interests"]?.ToString() ?? string.Empty;
            conn.Close();

            using var conn2 = new NpgsqlConnection(ConnectionString);
            var cmd2 = conn2.CreateCommand();
            cmd2.CommandText = @"
                SELECT u.id, u.name, u.email, u.password, u.created_at,
                       p.id as profile_id, p.program, p.interests, p.availability, p.updated_at
                FROM public.users u
                LEFT JOIN public.user_profiles p ON u.id = p.user_id
                WHERE u.id != @id
                AND (p.program = @program OR p.interests ILIKE '%' || @interests || '%')";

            cmd2.Parameters.AddWithValue("@id", userId);
            cmd2.Parameters.AddWithValue("@program", userProgram);
            cmd2.Parameters.AddWithValue("@interests", userInterests);

            var matchReader = GetData(conn2, cmd2);
            while (matchReader.Read())
            {
                matches.Add(ReadUserWithProfile(matchReader));
            }

            return matches;
        }

        public bool DeleteUser(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM public.users WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            return InsertData(conn, cmd);
        }

        public bool UpdateUser(User user)
        {
            try
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                conn.Open();
                using var transaction = conn.BeginTransaction();
                
                try
                {
                    // Update users table
                    var userCmd = conn.CreateCommand();
                    userCmd.Transaction = transaction;
                    userCmd.CommandText = @"
                        UPDATE public.users
                        SET name = @Name,
                            email = @Email
                        WHERE id = @Id";

                    userCmd.Parameters.AddWithValue("@Name", user.Name);
                    userCmd.Parameters.AddWithValue("@Email", user.Email);
                    userCmd.Parameters.AddWithValue("@Id", user.Id);

                    var userUpdated = userCmd.ExecuteNonQuery() > 0;
                    
                    // Update user_profiles table if profile exists
                    if (user.UserProfile != null)
                    {
                        var profileCmd = conn.CreateCommand();
                        profileCmd.Transaction = transaction;
                        profileCmd.CommandText = @"
                            UPDATE public.user_profiles
                            SET program = @Program,
                                interests = @Interests,
                                availability = @Availability,
                                updated_at = CURRENT_TIMESTAMP
                            WHERE user_id = @UserId";

                        profileCmd.Parameters.AddWithValue("@Program", user.UserProfile.Program ?? (object)DBNull.Value);
                        profileCmd.Parameters.AddWithValue("@Interests", user.UserProfile.Interests ?? (object)DBNull.Value);
                        profileCmd.Parameters.AddWithValue("@Availability", user.UserProfile.Availability ?? (object)DBNull.Value);
                        profileCmd.Parameters.AddWithValue("@UserId", user.Id);

                        profileCmd.ExecuteNonQuery();
                    }
                    
                    transaction.Commit();
                    return userUpdated;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUser: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string email) // Changed parameter name for clarity
        {
            // This remains synchronous but wrapped in a Task for signature compatibility
            // Assuming username is the email for now
            return await Task.FromResult(GetByEmail(email));
        }

        public User? GetByEmail(string email)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT u.id, u.name, u.email, u.password, u.created_at,
                       p.id as profile_id, p.program, p.interests, p.availability, p.updated_at
                FROM public.users u
                LEFT JOIN public.user_profiles p ON u.id = p.user_id
                WHERE u.email = @email";
            cmd.Parameters.AddWithValue("@email", email);
            
            var reader = GetData(conn, cmd);
            if (reader.Read())
            {
                return ReadUserWithProfile(reader);
            }
            return null;
        }

        public User? GetUserById(int id)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT u.id, u.name, u.email, u.password, u.created_at,
                       p.id as profile_id, p.program, p.interests, p.availability, p.updated_at
                FROM public.users u
                LEFT JOIN public.user_profiles p ON u.id = p.user_id
                WHERE u.id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read() ? ReadUserWithProfile(reader) : null;
        }

        private User ReadUserWithProfile(NpgsqlDataReader reader)
        {
            var user = new User
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Email = reader.GetString(reader.GetOrdinal("email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("password")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
            };

            // Check if profile data exists (LEFT JOIN might return null)
            if (!reader.IsDBNull(reader.GetOrdinal("profile_id")))
            {
                user.UserProfile = new UserProfile
                {
                    Id = reader.GetInt32(reader.GetOrdinal("profile_id")),
                    UserId = user.Id,
                    Program = reader.GetString(reader.GetOrdinal("program")),
                    Interests = reader.GetString(reader.GetOrdinal("interests")),
                    Availability = reader.GetString(reader.GetOrdinal("availability")),
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
                };
            }

            return user;
        }
    }
}
