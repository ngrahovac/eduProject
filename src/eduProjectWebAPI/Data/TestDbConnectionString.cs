using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class TestDbConnectionString : DbConnectionStringBase
    {
        public TestDbConnectionString()
        {
            ConnectionString = "server=localhost;database=eduproject_test;user=root";
        }
    }
}
