using eduProjectModel.Domain;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class ProjectsRepository : IProjectsRepository
    {
        //treba da ima sve CRUD metode, za sad u Z06 imamo samo citanje sto je potrebno za taj zahtjev

        private MySqlConnection dbConnection;

        public ProjectsRepository(MySqlConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public async Task<Project> GetAsync(int id) // FIX: make id long, dohvata projekat iz baze
        {
            Project project = new Project(); // TEST
            using (dbConnection)
            {
                await dbConnection.OpenAsync();
                MySqlCommand command = new MySqlCommand();
                command.Connection = dbConnection;

                // read project attributes from table `project`
                ReadBasicProjectInfo(command, id, ref project);

                if (project != null)
                {
                    // read collaborator profiles from  table `collaborator_profiles`
                    ReadCollaboratorProfilesInfo(command, id, project);

                    // read tag ids from table `project_tag`
                    ReadTagsInfo(command, id, project);

                    // read collaborator ids from table `project_collaborator`
                    ReadCollaboratorIds(command, id, project);
                }

                await dbConnection.CloseAsync();
            }
            return project;
        }
        private void ReadCollaboratorIds(MySqlCommand command, int id, Project project)
        {
            string readCollaboratorsCommandText = @"SELECT user_id
                                                    FROM project_collaborator
                                                    WHERE project_collaborator.project_id = @id";

            command.CommandText = readCollaboratorsCommandText;
            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter { DbType = DbType.Int32, ParameterName = "@id", Value = id });
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    int row = 0;
                    while (reader.Read())
                    {
                        project.CollaboratorIds.Add(reader.GetInt32(row++));
                    }
                }
            }

        }
        private void ReadTagsInfo(MySqlCommand command, int id, Project project)
        {
            string readTagsCommandText = @"SELECT name, description
                                           FROM tag
                                           INNER JOIN project_tag USING(tag_id)
                                           WHERE project_tag.project_id = @id";

            command.CommandText = readTagsCommandText;
            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter { DbType = DbType.Int32, ParameterName = "@id", Value = id });
            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        project.Tags.Add(new Tag(reader.GetString(0), ""));
                    }
                }
            }
        }
        private void ReadBasicProjectInfo(MySqlCommand command, int id, ref Project project)
        {
            string readProjectCommandText = @"SELECT project_id, title,  start_date, end_date, project.description, project.study_field_id, project_status_id, user_id,
                                                     study_field.name, study_field.description
                                              FROM project
                                              INNER JOIN study_field ON project.study_field_id = study_field.study_field_id
                                              WHERE project.project_id = @id";

            command.CommandText = readProjectCommandText;
            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter { DbType = DbType.Int32, ParameterName = "@id", Value = id });

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        project.ProjectId = reader.GetInt32(0);
                        project.Title = reader.GetString(1);

                        if (reader.IsDBNull(2))
                        {
                            project.StartDate = null;
                        }
                        else
                        {
                            project.StartDate = reader.GetDateTime(2);
                        }

                        if (reader.IsDBNull(3))
                        {
                            project.EndDate = null;
                        }
                        else
                        {
                            project.EndDate = reader.GetDateTime(3);
                        }

                        project.Description = reader.GetString(4);
                        project.ProjectStatus = (ProjectStatus)Enum.ToObject(typeof(ProjectStatus), reader.GetInt32(6));
                        project.AuthorId = reader.GetInt32(7);
                        project.StudyField = new StudyField() { Name = reader.GetString(8), Description = "" };
                    }
                }
                else
                    project = null;
            }

        }
        private void ReadCollaboratorProfilesInfo(MySqlCommand command, int id, Project project)
        {
            string readCollaboratorProfilesCommandText = @"SELECT collaborator_profile_id, collaborator_profile.description, user_account_type_id, 
	                                                       student_profile.cycle, study_year,
	                                                       faculty.name, study_program.name, study_program_specialization.name,       
	                                                       study_field.name, study_field.description
                                                           FROM collaborator_profile
                                                           LEFT OUTER JOIN student_profile USING(collaborator_profile_id)
                                                           LEFT OUTER JOIN faculty_member_profile USING(collaborator_profile_id)
                                                           LEFT OUTER JOIN faculty ON student_profile.faculty_id = faculty.faculty_id OR faculty_member_profile.faculty_id = faculty.faculty_id
                                                           LEFT OUTER JOIN study_program ON student_profile.study_program_id = study_program.study_program_id
                                                           LEFT OUTER JOIN study_program_specialization ON student_profile.study_program_specialization_id = study_program_specialization.study_program_specialization_id
                                                           LEFT OUTER JOIN study_field ON faculty_member_profile.study_field_id = study_field.study_field_id
                                                           WHERE project_id = 1";

            command.CommandText = readCollaboratorProfilesCommandText;
            command.Parameters.Clear();
            command.Parameters.Add(new MySqlParameter { DbType = DbType.Int32, ParameterName = "@id", Value = id });

            using (var reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CollaboratorProfileType profileType = (CollaboratorProfileType)Enum.ToObject(typeof(CollaboratorProfileType), reader.GetInt32(2));

                        if (profileType is CollaboratorProfileType.Student)
                        {
                            StudentProfile profile = new StudentProfile();
                            profile.CollaboratorProfileId = reader.GetInt32(0);
                            profile.Description = reader.GetString(1);
                            profile.StudyCycle = reader.GetInt32(3);
                            profile.StudyYear = reader.GetInt32(4);

                            profile.Faculty = new Faculty { Name = reader.GetString(5) };
                            StudyProgram program = new StudyProgram { Name = reader.GetString(6) };
                            profile.Faculty.AddStudyProgram(program);
                            profile.StudyProgram = program;

                            StudyProgramSpecialization programSpecialization = new StudyProgramSpecialization { Name = reader.GetString(7) };
                            program.AddProgramSpecialization(programSpecialization);
                            profile.StudyProgramSpecialization = programSpecialization;

                            project.CollaboratorProfiles.Add(profile);
                        }
                        else if (profileType is CollaboratorProfileType.FacultyMember)
                        {
                            FacultyMemberProfile profile = new FacultyMemberProfile();
                            profile.CollaboratorProfileId = reader.GetInt32(0);
                            profile.Faculty = new Faculty { Name = reader.GetString(5) };
                            profile.Description = reader.GetString(1);
                            profile.StudyField = new StudyField() { Name = reader.GetString(8), Description = "" };

                            project.CollaboratorProfiles.Add(profile);
                        }
                    }
                }
            }


        }
    }
}
