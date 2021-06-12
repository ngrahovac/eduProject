using eduProjectModel.Domain;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly DbConnectionParameters dbConnectionParameters;
        public UserSettingsRepository(DbConnectionParameters dbConnectionParameters)
        {
            this.dbConnectionParameters = dbConnectionParameters;
        }

        public async Task<UserSettings> GetAsync(int userId)
        {
            UserSettings settings = null;

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                    CommandText = @"SELECT email_visible, phone_visible, projects_visible,
                                           linkedin_profile, researchgate_profile, website,
                                           cv_link, photo_link, bio, tag_id
                                    FROM user_settings
                                    LEFT OUTER JOIN user_tag USING(user_id)
                                    WHERE user_id = @userId"
                };

                command.Parameters.Clear();
                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@userId",
                    DbType = DbType.Int32,
                    Value = userId
                });

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            if (settings == null)
                            {
                                settings = new UserSettings
                                {
                                    UserId = userId,
                                    EmailVisible = reader.GetBoolean(0),
                                    PhoneVisible = reader.GetBoolean(1),
                                    ProjectsVisible = reader.GetBoolean(2),
                                    LinkedinProfile = reader.IsDBNull(3) ? null : reader.GetString(3),
                                    ResearchgateProfile = reader.IsDBNull(4) ? null : reader.GetString(4),
                                    Website = reader.IsDBNull(5) ? null : reader.GetString(5),
                                    CvLink = reader.IsDBNull(6) ? null : reader.GetString(6),
                                    PhotoLink = reader.IsDBNull(7) ? null : reader.GetString(7),
                                    Bio = reader.IsDBNull(8) ? null : reader.GetString(8)
                                };
                            }

                            if (!reader.IsDBNull(9))
                            {
                                settings.UserTags.Add(Tag.tags[reader.GetInt32(9)]);
                            }
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return settings;
        }

        public async Task UpdateAsync(UserSettings settings)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection,
                };

                await connection.OpenAsync();

                await UpdateBasicInfo(command, settings);

                await UpdateTagsInfo(command, settings);

                await connection.CloseAsync();
            }
        }

        private async Task UpdateTagsInfo(MySqlCommand command, UserSettings settings)
        {

            command.CommandText = @"UPDATE user_settings
                                    SET
                                    email_visible = @emailVisible,
                                    phone_visible = @phoneVisible,
                                    projects_visible = @projectsVisible,
                                    linkedin_profile = @linkedinProfile,
                                    researchgate_profile = @researchgateProfile,
                                    website = @website,
                                    cv_link = @cvLink,
                                    photo_link = @photoLink,
                                    bio = @bio
                                    WHERE user_id = @userId";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@emailVisible",
                DbType = DbType.Boolean,
                Value = settings.EmailVisible
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@phoneVisible",
                DbType = DbType.Boolean,
                Value = settings.PhoneVisible
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@projectsVisible",
                DbType = DbType.Boolean,
                Value = settings.ProjectsVisible
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@linkedinProfile",
                DbType = DbType.String,
                Value = settings.LinkedinProfile
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@researchgateProfile",
                DbType = DbType.String,
                Value = settings.ResearchgateProfile
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@website",
                DbType = DbType.String,
                Value = settings.Website
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@cvLink",
                DbType = DbType.String,
                Value = settings.CvLink
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@photoLink",
                DbType = DbType.String,
                Value = settings.PhotoLink
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@bio",
                DbType = DbType.String,
                Value = settings.Bio
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@userId",
                DbType = DbType.Int32,
                Value = settings.UserId
            });

            await command.ExecuteNonQueryAsync();
        }

        private async Task UpdateBasicInfo(MySqlCommand command, UserSettings settings)
        {
            var currentSettings = await GetAsync(settings.UserId);

            var removedTags = currentSettings.UserTags.Where(t => !settings.UserTags.Contains(t));
            var newTags = settings.UserTags.Where(t => !currentSettings.UserTags.Contains(t));

            // remove deleted tags
            command.CommandText = "DELETE FROM user_tag WHERE user_id = @userId AND tag_id = @tagId";

            foreach (var tag in removedTags)
            {
                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@userId",
                    DbType = DbType.Int32,
                    Value = settings.UserId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@tagId",
                    DbType = DbType.Int32,
                    Value = Tag.tags.Where(t => t.Value.Name == tag.Name).First().Key
                });

                await command.ExecuteNonQueryAsync();
            }

            // insert new tags
            command.CommandText = @"INSERT INTO user_tag 
                                    (user_id, tag_id)
                                    VALUES
                                    (@userId, @tagId)";

            foreach (var tag in newTags)
            {
                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@userId",
                    DbType = DbType.Int32,
                    Value = settings.UserId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@tagId",
                    DbType = DbType.Int32,
                    Value = Tag.tags.Where(t => t.Value.Name == tag.Name).First().Key
                });

                await command.ExecuteNonQueryAsync();
            }
        }
    }
}

