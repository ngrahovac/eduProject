﻿using eduProjectModel.Domain;
using Microsoft.Extensions.Caching.Memory;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


namespace eduProjectWebAPI.Data
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly IMemoryCache cache;
        private readonly DbConnectionParameters dbConnectionParameters;

        public ProjectsRepository(DbConnectionParameters dbConnectionParameters, IMemoryCache cache)
        {
            this.dbConnectionParameters = dbConnectionParameters;
            this.cache = cache;
        }

        public async Task<Project> GetAsync(int id)
        {
            Project project = null;

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                // read project attributes from table `project`
                project = await ReadBasicProjectInfo(command, id);

                if (project != null)
                {
                    // read collaborator profiles from  table `collaborator_profiles`
                    await ReadCollaboratorProfilesInfo(command, id, project);

                    // read tag ids from table `project_tag`
                    await ReadTagsInfo(command, id, project);

                    // read collaborator ids from table `project_collaborator`
                    await ReadCollaboratorIds(command, id, project);
                }

                await connection.CloseAsync();
            }

            return project;
        }

        public async Task<ICollection<Project>> GetAllByAuthor(int authorId)
        {
            ICollection<Project> projects = new List<Project>();

            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    CommandText = @"SELECT project_id FROM project WHERE user_id = @authorId",
                    Connection = connection
                };

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    DbType = DbType.Int32,
                    ParameterName = "@authorId",
                    Value = authorId
                });

                await connection.OpenAsync();

                var projectIds = new List<int>();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            projectIds.Add(reader.GetInt32(0));
                        }
                    }
                }

                if (projectIds.Count > 0)
                {
                    foreach (var id in projectIds)
                    {
                        // read project attributes from table `project`
                        var project = await ReadBasicProjectInfo(command, id);

                        if (project != null)
                        {
                            // read collaborator profiles from  table `collaborator_profiles`
                            await ReadCollaboratorProfilesInfo(command, id, project);

                            // read tag ids from table `project_tag`
                            await ReadTagsInfo(command, id, project);

                            // read collaborator ids from table `project_collaborator`
                            await ReadCollaboratorIds(command, id, project);

                            projects.Add(project);
                        }
                    }
                }

                await connection.CloseAsync();
            }

            return projects;
        }

        private async Task<Project> ReadBasicProjectInfo(MySqlCommand command, int id)
        {
            Project project = new Project();

            command.CommandText = @"SELECT project_id, title, start_date, end_date, 
                                           project.description, project.study_field_id, 
                                           project_status_id, user_id
                                    FROM project
                                    WHERE project.project_id = @id";


            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@id",
                Value = id
            });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        project.ProjectId = reader.GetInt32(0);
                        project.Title = reader.GetString(1);
                        project.StartDate = !reader.IsDBNull(2) ? (DateTime?)reader.GetDateTime(2) : null;
                        project.EndDate = !reader.IsDBNull(3) ? (DateTime?)reader.GetDateTime(3) : null;
                        project.Description = reader.GetString(4);

                        int studyFieldId = reader.GetInt32(5);
                        project.StudyField = StudyField.fields[studyFieldId];

                        project.ProjectStatus = (ProjectStatus)Enum.ToObject(typeof(ProjectStatus), reader.GetInt32(6));
                        project.AuthorId = reader.GetInt32(7);
                    }
                }
                else
                    project = null;
            }

            return project;
        }

        private async Task ReadCollaboratorProfilesInfo(MySqlCommand command, int id, Project project)
        {
            command.CommandText = @"SELECT collaborator_profile_id, collaborator_profile.description, user_account_type_id, 
	                                       cycle, study_year, student_profile.faculty_id, study_program_id, study_program_specialization_id,
	                                       faculty_member_profile.faculty_id, study_field_id
                                           FROM collaborator_profile
                                           LEFT OUTER JOIN student_profile USING(collaborator_profile_id)
                                           LEFT OUTER JOIN faculty_member_profile USING(collaborator_profile_id)                                                           
                                           WHERE project_id = @id";

            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@id",
                Value = id
            });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        CollaboratorProfileType profileType = (CollaboratorProfileType)Enum.ToObject(typeof(CollaboratorProfileType), reader.GetInt32(2));

                        if (profileType is CollaboratorProfileType.Student)
                        {
                            StudentProfile profile = new StudentProfile
                            {
                                CollaboratorProfileId = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                StudyCycle = reader.GetInt32(3),
                                StudyYear = !reader.IsDBNull(4) ? (int?)reader.GetInt32(4) : null,
                                FacultyId = !reader.IsDBNull(5) ? (int?)reader.GetInt32(5) : null,
                                StudyProgramId = !reader.IsDBNull(6) ? (int?)reader.GetInt32(6) : null,
                                StudyProgramSpecializationId = !reader.IsDBNull(7) ? (int?)reader.GetInt32(7) : null
                            };

                            project.CollaboratorProfiles.Add(profile);
                        }

                        else if (profileType is CollaboratorProfileType.FacultyMember)
                        {
                            FacultyMemberProfile profile = new FacultyMemberProfile
                            {
                                CollaboratorProfileId = reader.GetInt32(0),
                                Description = reader.GetString(1),
                                FacultyId = !reader.IsDBNull(8) ? (int?)reader.GetInt32(8) : null,
                                StudyField = !reader.IsDBNull(9) ? StudyField.fields[reader.GetInt32(9)] : null
                            };

                            project.CollaboratorProfiles.Add(profile);
                        }
                    }
                }
            }
        }

        private async Task ReadTagsInfo(MySqlCommand command, int id, Project project)
        {
            command.CommandText = @"SELECT tag_id
                                    FROM project_tag
                                    WHERE project_id = @id";

            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@id",
                Value = id
            });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        int tagId = reader.GetInt32(0);
                        project.Tags.Add(Tag.tags[tagId]);
                    }
                }
            }
        }

        private async Task ReadCollaboratorIds(MySqlCommand command, int id, Project project)
        {
            command.CommandText = @"SELECT user_id
                                    FROM project_collaborator
                                    WHERE project_collaborator.project_id = @id";

            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter
            {
                DbType = DbType.Int32,
                ParameterName = "@id",
                Value = id
            });

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (reader.HasRows)
                {
                    while (await reader.ReadAsync())
                    {
                        project.CollaboratorIds.Add(reader.GetInt32(0));
                    }
                }
            }
        }

        public async Task AddAsync(Project project)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                await AddBasicProjectInfo(command, project);
                await AddCollaboratorProfilesInfo(command, project);
                await AddTagsInfo(command, project);

                await connection.CloseAsync();
            }
        }

        private async Task AddBasicProjectInfo(MySqlCommand command, Project project)
        {
            command.CommandText = @"INSERT INTO project
                                    (title, start_date, end_date, description, 
                                    study_field_id, project_status_id, user_id )
                                    VALUES
                                    (@title, @startDate, @endDate, @description,
                                    @fieldId, @statusId, @authorId)";

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = project.Title
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@startDate",
                DbType = DbType.DateTime,
                Value = project.StartDate
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@endDate",
                DbType = DbType.DateTime,
                Value = project.EndDate
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = project.Description
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fieldId",
                DbType = DbType.Int32,
                Value = StudyField.fields.Where(p => p.Value == project.StudyField).First().Key
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@statusId",
                DbType = DbType.Int32,
                Value = (int)project.ProjectStatus
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@authorId",
                DbType = DbType.Int32,
                Value = project.AuthorId
            });

            await command.ExecuteNonQueryAsync();
            project.ProjectId = (int)command.LastInsertedId; //TODO: make ID long

        }

        private async Task AddCollaboratorProfilesInfo(MySqlCommand command, Project project)
        {
            foreach (var profile in project.CollaboratorProfiles)
            {
                command.CommandText = @"INSERT INTO collaborator_profile
                                        (description, project_id, user_account_type_id)
                                        VALUES
                                        (@description, @projectId, @profileTypeId)";

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@description",
                    DbType = DbType.String,
                    Value = profile.Description
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@projectId",
                    DbType = DbType.Int32,
                    Value = project.ProjectId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@profileTypeId",
                    DbType = DbType.Int32,
                    Value = profile is StudentProfile ? (int)CollaboratorProfileType.Student : (int)CollaboratorProfileType.FacultyMember
                });

                await command.ExecuteNonQueryAsync();
                profile.CollaboratorProfileId = (int)command.LastInsertedId;

                if (profile is StudentProfile sp)
                {
                    command.CommandText = @"INSERT INTO student_profile
                                            (collaborator_profile_id, cycle, study_year,
                                            faculty_id, study_program_id, study_program_specialization_id)
                                            VALUES
                                            (@profileId, @cycle, @year,
                                            @facultyId, @programId, @specializationId)";

                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@profileId",
                        DbType = DbType.Int32,
                        Value = sp.CollaboratorProfileId
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@cycle",
                        DbType = DbType.Int32,
                        Value = sp.StudyCycle
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@year",
                        DbType = DbType.Int32,
                        Value = sp.StudyYear
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@facultyId",
                        DbType = DbType.Int32,
                        Value = sp.FacultyId
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@programId",
                        DbType = DbType.Int32,
                        Value = sp.StudyProgramId
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@specializationId",
                        DbType = DbType.Int32,
                        Value = sp.StudyProgramSpecializationId
                    });

                    await command.ExecuteNonQueryAsync();
                }
                else if (profile is FacultyMemberProfile fp)
                {
                    command.CommandText = @"INSERT INTO faculty_member_profile
                                            (collaborator_profile_id, faculty_id, study_field_id)
                                            VALUES
                                            (@profileId, @facultyId, @fieldId)";

                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@profileId",
                        DbType = DbType.Int32,
                        Value = fp.CollaboratorProfileId
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@facultyId",
                        DbType = DbType.Int32,
                        Value = fp.FacultyId
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@fieldId",
                        DbType = DbType.Int32,
                        Value = StudyField.fields.Where(p => p.Value == fp.StudyField).First().Key
                    });

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task AddTagsInfo(MySqlCommand command, Project project)
        {
            foreach (var tag in project.Tags)
            {
                command.CommandText = @"INSERT INTO project_tag
                                        (project_id, tag_id)
                                        VALUES
                                        (@projectId, @tagId)";

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@projectId",
                    DbType = DbType.Int32,
                    Value = project.ProjectId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@tagId",
                    DbType = DbType.Int32,
                    Value = Tag.tags.Where(p => p.Value == tag).First().Key
                });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateAsync(Project updatedProject)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                var project = await GetAsync(updatedProject.ProjectId);

                if (project.Title != updatedProject.Title ||
                    project.Description != updatedProject.Description ||
                    project.StartDate != updatedProject.StartDate ||
                    project.StudyField != updatedProject.StudyField ||
                    project.ProjectStatus != updatedProject.ProjectStatus)
                {
                    await UpdateBasicProjectInfo(command, updatedProject);
                }

                if (project.CollaboratorProfiles.Count != updatedProject.CollaboratorProfiles.Count) // profiles can only be added
                {
                    await UpdateCollaboratorProfilesInfo(command, updatedProject);
                }

                if (updatedProject.CollaboratorIds.Count > 0)
                {
                    await UpdateCollaboratorsInfo(command, updatedProject);
                }

                await UpdateTagsInfo(command, updatedProject);

                await connection.CloseAsync();
            }
        }

        private async Task UpdateBasicProjectInfo(MySqlCommand command, Project project)
        {
            command.CommandText = @"UPDATE project
                                    SET
                                    title = @title,
                                    start_date = @startDate,
                                    end_date = @endDate,
                                    description = @description,
                                    study_field_id = @fieldId,
                                    project_status_id = @statusId
                                    WHERE project_id = @projectId";

            // add project_status_id = @statusId

            command.Parameters.Clear();

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@title",
                DbType = DbType.String,
                Value = project.Title
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@startDate",
                DbType = DbType.DateTime,
                Value = project.StartDate
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@endDate",
                DbType = DbType.DateTime,
                Value = project.EndDate
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@description",
                DbType = DbType.String,
                Value = project.Description
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@statusId",
                DbType = DbType.Int32,
                Value = (int)project.ProjectStatus
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@fieldId",
                DbType = DbType.Int32,
                Value = StudyField.fields.Where(p => p.Value == project.StudyField).First().Key
            });

            command.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@projectId",
                DbType = DbType.Int32,
                Value = project.ProjectId
            });

            await command.ExecuteNonQueryAsync();
        }

        private async Task UpdateCollaboratorProfilesInfo(MySqlCommand command, Project project)
        {
            var oldProject = await GetAsync(project.ProjectId);

            foreach (var profile in project.CollaboratorProfiles)
            {
                if (!oldProject.CollaboratorProfiles.Select(p => p.CollaboratorProfileId).Contains(profile.CollaboratorProfileId))
                {
                    // insert new profile into database

                    command.CommandText = @"INSERT INTO collaborator_profile
                                        (description, project_id, user_account_type_id)
                                        VALUES
                                        (@description, @projectId, @profileTypeId)";

                    command.Parameters.Clear();

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@description",
                        DbType = DbType.String,
                        Value = profile.Description
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@projectId",
                        DbType = DbType.Int32,
                        Value = project.ProjectId
                    });

                    command.Parameters.Add(new MySqlParameter
                    {
                        ParameterName = "@profileTypeId",
                        DbType = DbType.Int32,
                        Value = profile is StudentProfile ? (int)CollaboratorProfileType.Student : (int)CollaboratorProfileType.FacultyMember
                    });

                    await command.ExecuteNonQueryAsync();
                    profile.CollaboratorProfileId = (int)command.LastInsertedId;

                    if (profile is StudentProfile sp)
                    {
                        command.CommandText = @"INSERT INTO student_profile
                                            (collaborator_profile_id, cycle, study_year,
                                            faculty_id, study_program_id, study_program_specialization_id)
                                            VALUES
                                            (@profileId, @cycle, @year,
                                            @facultyId, @programId, @specializationId)";

                        command.Parameters.Clear();

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@profileId",
                            DbType = DbType.Int32,
                            Value = sp.CollaboratorProfileId
                        });

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@cycle",
                            DbType = DbType.Int32,
                            Value = sp.StudyCycle
                        });

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@year",
                            DbType = DbType.Int32,
                            Value = sp.StudyYear
                        });

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@facultyId",
                            DbType = DbType.Int32,
                            Value = sp.FacultyId
                        });

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@programId",
                            DbType = DbType.Int32,
                            Value = sp.StudyProgramId
                        });

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@specializationId",
                            DbType = DbType.Int32,
                            Value = sp.StudyProgramSpecializationId
                        });

                        await command.ExecuteNonQueryAsync();
                    }
                    else if (profile is FacultyMemberProfile fp)
                    {
                        command.CommandText = @"INSERT INTO faculty_member_profile
                                            (collaborator_profile_id, faculty_id, study_field_id)
                                            VALUES
                                            (@profileId, @facultyId, @fieldId)";

                        command.Parameters.Clear();

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@profileId",
                            DbType = DbType.Int32,
                            Value = fp.CollaboratorProfileId
                        });

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@facultyId",
                            DbType = DbType.Int32,
                            Value = fp.FacultyId
                        });

                        command.Parameters.Add(new MySqlParameter
                        {
                            ParameterName = "@fieldId",
                            DbType = DbType.Int32,
                            Value = StudyField.fields.Where(p => p.Value == fp.StudyField).First().Key
                        });

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }

        }

        private async Task UpdateTagsInfo(MySqlCommand command, Project project)
        {
            var oldProject = await GetAsync(project.ProjectId);

            // FIX: works because Tag instances are fetched and shared from a pool
            var addedTags = project.Tags.Where(t => !oldProject.Tags.Contains(t));
            var removedTags = oldProject.Tags.Where(t => !project.Tags.Contains(t));

            var addedTagIds = addedTags.Select(t => Tag.tags.Where(p => p.Value == t).First().Key);
            var removedTagIds = removedTags.Select(t => Tag.tags.Where(p => p.Value == t).First().Key);

            foreach (var id in addedTagIds)
            {
                command.CommandText = @"INSERT INTO project_tag
                                        (project_id, tag_id)
                                        VALUES
                                        (@projectId, @tagId)";

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@projectId",
                    DbType = DbType.Int32,
                    Value = project.ProjectId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@tagId",
                    DbType = DbType.Int32,
                    Value = id
                });

                await command.ExecuteNonQueryAsync();
            }

            foreach (var id in removedTagIds)
            {
                command.CommandText = @"DELETE FROM project_tag
                                        WHERE project_id = @projectId
                                        AND tag_id = @tagId";

                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@projectId",
                    DbType = DbType.Int32,
                    Value = project.ProjectId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@tagId",
                    DbType = DbType.Int32,
                    Value = id
                });

                await command.ExecuteNonQueryAsync();
            }
        }

        private async Task UpdateCollaboratorsInfo(MySqlCommand command, Project project)
        {
            command.CommandText = @"INSERT INTO project_collaborator
                                    (project_id, user_id)
                                    VALUES
                                    (@projectId, @userId)";

            foreach (var id in project.CollaboratorIds)
            {
                command.Parameters.Clear();

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@projectId",
                    DbType = DbType.Int32,
                    Value = project.ProjectId
                });

                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@userId",
                    DbType = DbType.Int32,
                    Value = id
                });

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAsync(Project project)
        {
            using (var connection = new MySqlConnection(dbConnectionParameters.ConnectionString))
            {
                MySqlCommand command = new MySqlCommand
                {
                    Connection = connection
                };

                await connection.OpenAsync();

                command.CommandText = @"DELETE FROM project_collaborator WHERE project_id = @id";
                command.Parameters.Add(new MySqlParameter
                {
                    ParameterName = "@id",
                    DbType = DbType.Int32,
                    Value = project.ProjectId
                });
                await command.ExecuteNonQueryAsync();

                command.CommandText = @"DELETE FROM project_tag WHERE project_id = @id";
                await command.ExecuteNonQueryAsync();

                command.CommandText = @"DELETE project_application
                                        FROM project
                                        INNER JOIN collaborator_profile USING(project_id)
                                        LEFT OUTER JOIN project_application USING(collaborator_profile_id)
                                        WHERE project_id = @id";
                await command.ExecuteNonQueryAsync();

                // delete applications for collaborator profiles of this project
                command.CommandText = @"DELETE student_profile, faculty_member_profile
                                        FROM project
                                        INNER JOIN collaborator_profile USING(project_id)
                                        LEFT OUTER JOIN student_profile USING(collaborator_profile_id)
                                        LEFT OUTER JOIN faculty_member_profile USING(collaborator_profile_id)
                                        LEFT OUTER JOIN project_application USING(collaborator_profile_id)
                                        WHERE project.project_id = @id";
                await command.ExecuteNonQueryAsync();

                command.CommandText = @"DELETE FROM collaborator_profile WHERE project_id = @id";
                await command.ExecuteNonQueryAsync();

                command.CommandText = @"DELETE FROM project WHERE project_id = @id";
                await command.ExecuteNonQueryAsync();

                await connection.CloseAsync();
            }
        }
    }
}
