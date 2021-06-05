namespace eduProjectWebAPI.Data
{
    public class TestDbConnectionParameters : DbConnectionParameters
    {
        public TestDbConnectionParameters()
        {
            ConnectionString = "server=localhost;database=eduproject_test;user=root";
        }
    }
}
