using BuddyMatch.Model.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuddyMatch.Model.Repositories
{
    public class UserProfileRepository : BaseRepository
    {
        public UserProfileRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<UserProfile?> GetByUserIdAsync(int userId)
        {
            return await Task.FromResult(GetByUserId(userId));
        }

        public UserProfile? GetByUserId(int userId)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT * FROM public.user_profiles WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);
            
            var reader = GetData(conn, cmd);
            if (reader.Read())
            {
                return ReadUserProfile(reader);
            }
            return null;
        }

        public async Task<bool> CreateUserProfileAsync(UserProfile userProfile)
        {
            return await Task.FromResult(InsertUserProfile(userProfile));
        }

        public bool InsertUserProfile(UserProfile userProfile)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO public.user_profiles (user_id, program, interests, availability)
                VALUES (@user_id, @program, @interests, @availability)";

            cmd.Parameters.AddWithValue("@user_id", userProfile.UserId);
            cmd.Parameters.AddWithValue("@program", NpgsqlDbType.Text, userProfile.Program ?? string.Empty);
            cmd.Parameters.AddWithValue("@interests", NpgsqlDbType.Text, userProfile.Interests ?? string.Empty);
            cmd.Parameters.AddWithValue("@availability", NpgsqlDbType.Text, userProfile.Availability ?? string.Empty);

            return InsertData(conn, cmd);
        }

        public bool UpdateUserProfile(UserProfile userProfile)
        {
            try
            {
                using var conn = new NpgsqlConnection(ConnectionString);
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    UPDATE public.user_profiles
                    SET program = @Program,
                        interests = @Interests,
                        availability = @Availability,
                        updated_at = CURRENT_TIMESTAMP
                    WHERE user_id = @UserId";

                cmd.Parameters.AddWithValue("@Program", userProfile.Program ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Interests", userProfile.Interests ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Availability", userProfile.Availability ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@UserId", userProfile.UserId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateUserProfile: {ex.Message}");
                return false;
            }
        }

        public bool DeleteUserProfile(int userId)
        {
            using var conn = new NpgsqlConnection(ConnectionString);
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM public.user_profiles WHERE user_id = @user_id";
            cmd.Parameters.AddWithValue("@user_id", userId);

            return InsertData(conn, cmd);
        }

        private UserProfile ReadUserProfile(NpgsqlDataReader reader)
        {
            return new UserProfile
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                Program = reader.GetString(reader.GetOrdinal("program")),
                Interests = reader.GetString(reader.GetOrdinal("interests")),
                Availability = reader.GetString(reader.GetOrdinal("availability")),
                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at"))
            };
        }
    }
}
