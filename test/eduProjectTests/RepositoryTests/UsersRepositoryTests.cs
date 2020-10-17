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
    public class UsersRepositoryTests
    {
        private readonly IUsersRepository users;

        public UsersRepositoryTests()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            users = new UsersRepository(new TestDbConnectionString(), memoryCache);
        }

        [Fact]
        public async Task GetById_IdIsNonExisting_ReturnNull()
        {
            var result = await users.GetAsync(0);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetById_IdExists_ReturnUser()
        {
            var result = await users.GetAsync(1);
            Assert.IsAssignableFrom<User>(result);
        }
    }
}
