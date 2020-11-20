using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eduProjectTests.RepositoryTests
{
    public class ProjectApplicationsRepositoryTests
    {
        private readonly IProjectApplicationsRepository applications;

        public ProjectApplicationsRepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            applications = new ProjectApplicationsRepository(new TestDbConnectionParameters());

            StudyField.fields.Clear();
            Tag.tags.Clear();

            for (int i = 1; i < 11; i++)
            {
                StudyField.fields.Add(i, new StudyField { Name = "test field" });
                Tag.tags.Add(i, new Tag { Name = "test tag" });
            }

            /*
            var testDbController = new TestDatabaseController(new TestDbConnectionParameters());
            Task.Run(() => testDbController.SeedData()).Wait();
            */
        }

        [Fact]
        public async void GetById_IdExists_ReturnApplication()
        {
            var result = await applications.GetById(1);
            Assert.IsType<ProjectApplication>(result);
        }

        [Fact]
        public async void GetById_IdIsNonExisting_ReturnNull()
        {
            var result = await applications.GetById(0);
            Assert.Null(result);
        }

        [Fact]
        public async void GetByApplicant_ApplicationsExist_ReturnApplications()
        {
            var result = await applications.GetByApplicantId(6);
            var count = result.Count;
            Assert.Equal(2, count);
        }

        [Fact]
        public async void GetByProject_ApplicationsExist_ReturnApplications()
        {
            var result = await applications.GetByProjectId(1);
            var count = result.Count;
            Assert.Equal(5, count);
        }

    }
}
