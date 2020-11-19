using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eduProjectWebAPI.Data
{
    public class DevelopmentDbConnectionParameters : DbConnectionParameters
    {
        public DevelopmentDbConnectionParameters(IConfiguration configuration)
        {
            ConnectionString = configuration["ConnectionStrings:eduProjectDb"];
        }
    }
}
