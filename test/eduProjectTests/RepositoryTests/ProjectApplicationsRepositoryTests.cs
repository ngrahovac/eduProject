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

            applications = new ProjectApplicationsRepository(new TestDbConnectionString());

            StudyField.fields.Clear();
            Tag.tags.Clear();

            for (int i = 1; i < 11; i++)
            {
                StudyField.fields.Add(i, new StudyField { Name = "test field" });
                Tag.tags.Add(i, new Tag { Name = "test tag" });
            }

            var testDbController = new TestDatabaseController(new TestDbConnectionString());
            Task.Run(() => testDbController.SeedData()).Wait();
        }


    }
}
