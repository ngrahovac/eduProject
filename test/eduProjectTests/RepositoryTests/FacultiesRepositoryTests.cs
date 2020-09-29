using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eduProjectTests.RepositoryTests
{
    public class FacultiesRepositoryTests
    {
        private readonly IFacultiesRepository faculties;

        public FacultiesRepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            faculties = new FacultiesRepository(new TestDbConnectionString(), memoryCache);
        }

        [Fact]
        public async Task GetById_IdIsNonExisting_ReturnNull()
        {
            var result = await faculties.GetAsync(0);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_IdExists_ReturnFaculty()
        {
            var result = await faculties.GetAsync(1);
            Assert.IsType<Faculty>(result);
        }
    }
}
