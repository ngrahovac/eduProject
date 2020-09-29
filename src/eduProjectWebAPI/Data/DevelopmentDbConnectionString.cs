using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class DevelopmentDbConnectionString : DbConnectionStringBase
    {
        public DevelopmentDbConnectionString(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:eduProjectDb"];
        }
    }
}
