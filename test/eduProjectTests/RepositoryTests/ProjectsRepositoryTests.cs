﻿using eduProjectModel.Domain;
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
    public class ProjectsRepositoryTests
    {
        private readonly IProjectsRepository projects;

        public ProjectsRepositoryTests()
        {
            // TODO: add mocked cache as dependency
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            projects = new ProjectsRepository(new TestDbConnectionString(), memoryCache);
        }

        [Fact]
        public async Task GetById_IdIsNonExisting_ReturnNotFound()
        {
            var result = await projects.GetAsync(0);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_IdExists_ReturnProject()
        {
            var result = await projects.GetAsync(1);
            Assert.IsType<Project>(result);
        }
    }
}
