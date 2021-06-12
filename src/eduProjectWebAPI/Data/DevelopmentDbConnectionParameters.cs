using Microsoft.Extensions.Configuration;

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
