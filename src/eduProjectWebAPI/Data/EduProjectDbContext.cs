using System;
using eduProjectWebAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eduProjectWebAPI
{
    public partial class EduProjectDbContext : DbContext
    {
        public EduProjectDbContext()
        {
        }

        public EduProjectDbContext(DbContextOptions<EduProjectDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AcademicRank> AcademicRank { get; set; }
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<CollaboratorProfile> CollaboratorProfile { get; set; }
        public virtual DbSet<Faculty> Faculty { get; set; }
        public virtual DbSet<FacultyMember> FacultyMember { get; set; }
        public virtual DbSet<FacultyMemberProfile> FacultyMemberProfile { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<ProjectApplication> ProjectApplication { get; set; }
        public virtual DbSet<ProjectApplicationStatus> ProjectApplicationStatus { get; set; }
        public virtual DbSet<ProjectCollaborator> ProjectCollaborator { get; set; }
        public virtual DbSet<ProjectStatus> ProjectStatus { get; set; }
        public virtual DbSet<ProjectTag> ProjectTag { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<StudentProfile> StudentProfile { get; set; }
        public virtual DbSet<StudyField> StudyField { get; set; }
        public virtual DbSet<StudyProgram> StudyProgram { get; set; }
        public virtual DbSet<StudyProgramSpecialization> StudyProgramSpecialization { get; set; }
        public virtual DbSet<Tag> Tag { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserAccountType> UserAccountType { get; set; }
        public virtual DbSet<UserSettings> UserSettings { get; set; }
        public virtual DbSet<UserTag> UserTag { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=eduProjectDb", x => x.ServerVersion("5.6.40-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AcademicRank>(entity =>
            {
                entity.ToTable("academic_rank");

                entity.HasIndex(x => x.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.AcademicRankId)
                    .HasColumnName("academic_rank_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(x => x.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("account");

                entity.HasIndex(x => x.Salt)
                    .HasName("salt_UNIQUE")
                    .IsUnique();

                entity.HasIndex(x => x.UserAccountTypeId)
                    .HasName("fk_account_user_account_type1_idx");

                entity.HasIndex(x => x.Username)
                    .HasName("username_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(80)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("password_hash")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.Salt)
                    .HasColumnName("salt")
                    .HasColumnType("varchar(12)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.UserAccountTypeId)
                    .HasColumnName("user_account_type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.HasOne(d => d.UserAccountType)
                    .WithMany(p => p.Account)
                    .HasForeignKey(x => x.UserAccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_account_user_account_type1");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Account)
                    .HasForeignKey<Account>(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_account_user1");
            });

            modelBuilder.Entity<CollaboratorProfile>(entity =>
            {
                entity.ToTable("collaborator_profile");

                entity.HasIndex(x => x.ProjectId)
                    .HasName("fk_collaborator_profile_project1_idx");

                entity.HasIndex(x => x.UserAccountTypeId)
                    .HasName("fk_collaborator_profile_user_account_type1_idx");

                entity.Property(e => e.CollaboratorProfileId)
                    .HasColumnName("collaborator_profile_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(200)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserAccountTypeId)
                    .HasColumnName("user_account_type_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.CollaboratorProfile)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_collaborator_profile_project1");

                entity.HasOne(d => d.UserAccountType)
                    .WithMany(p => p.CollaboratorProfile)
                    .HasForeignKey(x => x.UserAccountTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_collaborator_profile_user_account_type1");
            });

            modelBuilder.Entity<Faculty>(entity =>
            {
                entity.ToTable("faculty");

                entity.HasIndex(x => x.Name)
                    .HasName("name_idx");

                entity.Property(e => e.FacultyId)
                    .HasColumnName("faculty_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<FacultyMember>(entity =>
            {
                entity.HasKey(x => x.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("faculty_member");

                entity.HasIndex(x => x.AcademicRankId)
                    .HasName("fk_faculty_member_academic_rank1_idx");

                entity.HasIndex(x => x.FacultyId)
                    .HasName("fk_faculty_member_faculty1_idx");

                entity.HasIndex(x => x.StudyFieldId)
                    .HasName("fk_faculty_member_study_field1_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AcademicRankId)
                    .HasColumnName("academic_rank_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FacultyId)
                    .HasColumnName("faculty_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyFieldId)
                    .HasColumnName("study_field_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.AcademicRank)
                    .WithMany(p => p.FacultyMember)
                    .HasForeignKey(x => x.AcademicRankId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_faculty_member_academic_rank1");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.FacultyMember)
                    .HasForeignKey(x => x.FacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_faculty_member_faculty1");

                entity.HasOne(d => d.StudyField)
                    .WithMany(p => p.FacultyMember)
                    .HasForeignKey(x => x.StudyFieldId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_faculty_member_study_field1");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.FacultyMember)
                    .HasForeignKey<FacultyMember>(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_faculty_member_user1");
            });

            modelBuilder.Entity<FacultyMemberProfile>(entity =>
            {
                entity.HasKey(x => x.CollaboratorProfileId)
                    .HasName("PRIMARY");

                entity.ToTable("faculty_member_profile");

                entity.HasIndex(x => x.FacultyId)
                    .HasName("fk_faculty_member_profile_faculty1_idx");

                entity.HasIndex(x => x.StudyFieldId)
                    .HasName("fk_faculty_member_profile_study_field1_idx");

                entity.Property(e => e.CollaboratorProfileId)
                    .HasColumnName("collaborator_profile_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FacultyId)
                    .HasColumnName("faculty_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyFieldId)
                    .HasColumnName("study_field_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.CollaboratorProfile)
                    .WithOne(p => p.FacultyMemberProfile)
                    .HasForeignKey<FacultyMemberProfile>(x => x.CollaboratorProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_faculty_member_profile_profile_collaborator_profile1");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.FacultyMemberProfile)
                    .HasForeignKey(x => x.FacultyId)
                    .HasConstraintName("fk_faculty_member_profile_faculty1");

                entity.HasOne(d => d.StudyField)
                    .WithMany(p => p.FacultyMemberProfile)
                    .HasForeignKey(x => x.StudyFieldId)
                    .HasConstraintName("fk_faculty_member_profile_study_field1");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("project");

                entity.HasIndex(x => x.ProjectStatusId)
                    .HasName("fk_project_project_status1_idx");

                entity.HasIndex(x => x.StudyFieldId)
                    .HasName("fk_project_study_field1_idx");

                entity.HasIndex(x => x.UserId)
                    .HasName("fk_project_user1_idx");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("date");

                entity.Property(e => e.ProjectStatusId)
                    .HasColumnName("project_status_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("date");

                entity.Property(e => e.StudyFieldId)
                    .HasColumnName("study_field_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.ProjectStatus)
                    .WithMany(p => p.Project)
                    .HasForeignKey(x => x.ProjectStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_project_status1");

                entity.HasOne(d => d.StudyField)
                    .WithMany(p => p.Project)
                    .HasForeignKey(x => x.StudyFieldId)
                    .HasConstraintName("fk_project_study_field1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Project)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_user1");
            });

            modelBuilder.Entity<ProjectApplication>(entity =>
            {
                entity.ToTable("project_application");

                entity.HasIndex(x => x.CollaboratorProfileId)
                    .HasName("fk_project_application_collaborator_profile1_idx");

                entity.HasIndex(x => x.ProjectApplicationStatusId)
                    .HasName("fk_project_application_project_application_status1_idx");

                entity.HasIndex(x => x.UserId)
                    .HasName("fk_project_application_user1_idx");

                entity.Property(e => e.ProjectApplicationId)
                    .HasColumnName("project_application_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ApplicantComment)
                    .HasColumnName("applicant_comment")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.AuthorComment)
                    .HasColumnName("author_comment")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.CollaboratorProfileId)
                    .HasColumnName("collaborator_profile_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProjectApplicationStatusId)
                    .HasColumnName("project_application_status_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.CollaboratorProfile)
                    .WithMany(p => p.ProjectApplication)
                    .HasForeignKey(x => x.CollaboratorProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_application_collaborator_profile1");

                entity.HasOne(d => d.ProjectApplicationStatus)
                    .WithMany(p => p.ProjectApplication)
                    .HasForeignKey(x => x.ProjectApplicationStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_application_project_application_status1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectApplication)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_application_user1");
            });

            modelBuilder.Entity<ProjectApplicationStatus>(entity =>
            {
                entity.ToTable("project_application_status");

                entity.HasIndex(x => x.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ProjectApplicationStatusId)
                    .HasColumnName("project_application_status_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<ProjectCollaborator>(entity =>
            {
                entity.HasKey(x => new { x.ProjectId, x.UserId })
                    .HasName("PRIMARY");

                entity.ToTable("project_collaborator");

                entity.HasIndex(x => x.ProjectId)
                    .HasName("fk_project_collaborator_project1_idx");

                entity.HasIndex(x => x.UserId)
                    .HasName("fk_project_collaborator_user1_idx");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectCollaborator)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_collaborator_project1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectCollaborator)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_collaborator_user1");
            });

            modelBuilder.Entity<ProjectStatus>(entity =>
            {
                entity.ToTable("project_status");

                entity.HasIndex(x => x.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ProjectStatusId)
                    .HasColumnName("project_status_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<ProjectTag>(entity =>
            {
                entity.HasKey(x => new { x.ProjectId, x.TagId })
                    .HasName("PRIMARY");

                entity.ToTable("project_tag");

                entity.HasIndex(x => x.ProjectId)
                    .HasName("fk_project_tag_project1_idx");

                entity.HasIndex(x => x.TagId)
                    .HasName("fk_project_tag_tag1_idx");

                entity.Property(e => e.ProjectId)
                    .HasColumnName("project_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TagId)
                    .HasColumnName("tag_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectTag)
                    .HasForeignKey(x => x.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_tag_project1");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.ProjectTag)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_project_tag_tag1");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(x => x.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("student");

                entity.HasIndex(x => x.StudyProgramId)
                    .HasName("fk_student_study_program1_idx");

                entity.HasIndex(x => x.StudyProgramSpecializationId)
                    .HasName("fk_student_study_program_specialization1_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyProgramId)
                    .HasColumnName("study_program_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyProgramSpecializationId)
                    .HasColumnName("study_program_specialization_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyYear)
                    .HasColumnName("study_year")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.StudyProgram)
                    .WithMany(p => p.Student)
                    .HasForeignKey(x => x.StudyProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_study_program1");

                entity.HasOne(d => d.StudyProgramSpecialization)
                    .WithMany(p => p.Student)
                    .HasForeignKey(x => x.StudyProgramSpecializationId)
                    .HasConstraintName("fk_student_study_program_specialization1");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_user1");
            });

            modelBuilder.Entity<StudentProfile>(entity =>
            {
                entity.HasKey(x => x.CollaboratorProfileId)
                    .HasName("PRIMARY");

                entity.ToTable("student_profile");

                entity.HasIndex(x => x.FacultyId)
                    .HasName("fk_student_profile_faculty1_idx");

                entity.HasIndex(x => x.StudyProgramId)
                    .HasName("fk_student_profile_study_program1_idx");

                entity.HasIndex(x => x.StudyProgramSpecializationId)
                    .HasName("fk_student_profile_study_program_specialization1_idx");

                entity.Property(e => e.CollaboratorProfileId)
                    .HasColumnName("collaborator_profile_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cycle)
                    .HasColumnName("cycle")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FacultyId)
                    .HasColumnName("faculty_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyProgramId)
                    .HasColumnName("study_program_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyProgramSpecializationId)
                    .HasColumnName("study_program_specialization_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.StudyYear)
                    .HasColumnName("study_year")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.CollaboratorProfile)
                    .WithOne(p => p.StudentProfile)
                    .HasForeignKey<StudentProfile>(x => x.CollaboratorProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_student_profile_collaborator_profile1");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.StudentProfile)
                    .HasForeignKey(x => x.FacultyId)
                    .HasConstraintName("fk_student_profile_faculty1");

                entity.HasOne(d => d.StudyProgram)
                    .WithMany(p => p.StudentProfile)
                    .HasForeignKey(x => x.StudyProgramId)
                    .HasConstraintName("fk_student_profile_study_program1");

                entity.HasOne(d => d.StudyProgramSpecialization)
                    .WithMany(p => p.StudentProfile)
                    .HasForeignKey(x => x.StudyProgramSpecializationId)
                    .HasConstraintName("fk_student_profile_study_program_specialization1");
            });

            modelBuilder.Entity<StudyField>(entity =>
            {
                entity.ToTable("study_field");

                entity.HasIndex(x => x.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.StudyFieldId)
                    .HasColumnName("study_field_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<StudyProgram>(entity =>
            {
                entity.ToTable("study_program");

                entity.HasIndex(x => x.FacultyId)
                    .HasName("fk_study_program_faculty1_idx");

                entity.HasIndex(x => x.Name)
                    .HasName("name_idx");

                entity.Property(e => e.StudyProgramId)
                    .HasColumnName("study_program_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Cycle)
                    .HasColumnName("cycle")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.DurationYears)
                    .HasColumnName("duration_years")
                    .HasColumnType("tinyint(4)");

                entity.Property(e => e.FacultyId)
                    .HasColumnName("faculty_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.HasOne(d => d.Faculty)
                    .WithMany(p => p.StudyProgram)
                    .HasForeignKey(x => x.FacultyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_study_program_faculty1");
            });

            modelBuilder.Entity<StudyProgramSpecialization>(entity =>
            {
                entity.ToTable("study_program_specialization");

                entity.HasIndex(x => x.Name)
                    .HasName("name_idx");

                entity.HasIndex(x => x.StudyProgramId)
                    .HasName("fk_study_program_specialization_study_program1_idx");

                entity.Property(e => e.StudyProgramSpecializationId)
                    .HasColumnName("study_program_specialization_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.StudyProgramId)
                    .HasColumnName("study_program_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.StudyProgram)
                    .WithMany(p => p.StudyProgramSpecialization)
                    .HasForeignKey(x => x.StudyProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_study_program_specialization_study_program1");
            });

            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("tag");

                entity.HasIndex(x => x.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.TagId)
                    .HasColumnName("tag_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasColumnType("varchar(50)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.PhoneFormat)
                    .HasColumnName("phone_format")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.PhoneNumber)
                    .HasColumnName("phone_number")
                    .HasColumnType("varchar(40)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<UserAccountType>(entity =>
            {
                entity.ToTable("user_account_type");

                entity.HasIndex(x => x.Name)
                    .HasName("name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserAccountTypeId)
                    .HasColumnName("user_account_type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(30)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");
            });

            modelBuilder.Entity<UserSettings>(entity =>
            {
                entity.HasKey(x => x.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("user_settings");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.AccountPhoto)
                    .HasColumnName("account_photo")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.AreProjectsVisible)
                    .HasColumnName("are_projects_visible")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Bio)
                    .HasColumnName("bio")
                    .HasColumnType("varchar(2000)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.Cv)
                    .HasColumnName("cv")
                    .HasColumnType("varchar(500)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.IsEmailVisible)
                    .HasColumnName("is_email_visible")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.IsPhoneVisible)
                    .HasColumnName("is_phone_visible")
                    .HasColumnType("tinyint(4)")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.LinkedinProfile)
                    .HasColumnName("linkedin_profile")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.ResearchgateProfile)
                    .HasColumnName("researchgate_profile")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.Property(e => e.Website)
                    .HasColumnName("website")
                    .HasColumnType("varchar(100)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_unicode_520_ci");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserSettings)
                    .HasForeignKey<UserSettings>(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_settings_user1");
            });

            modelBuilder.Entity<UserTag>(entity =>
            {
                entity.HasKey(x => new { x.UserId, x.TagId })
                    .HasName("PRIMARY");

                entity.ToTable("user_tag");

                entity.HasIndex(x => x.TagId)
                    .HasName("fk_user_tag_tag1_idx");

                entity.HasIndex(x => x.UserId)
                    .HasName("fk_user_tag_user1_idx");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.TagId)
                    .HasColumnName("tag_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Tag)
                    .WithMany(p => p.UserTag)
                    .HasForeignKey(x => x.TagId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_tag_tag1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserTag)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_user_tag_user1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
