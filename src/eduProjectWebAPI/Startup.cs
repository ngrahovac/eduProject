using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eduProjectWebAPI
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // TEST ONLY
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });

            services.AddControllers();

            services.AddMemoryCache();

            ConfigureDataLayerServices(services);
        }

        private void ConfigureDataLayerServices(IServiceCollection services)
        {
            services.AddTransient<IProjectsRepository, ProjectsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IFacultiesRepository, FacultiesRepository>();
            services.AddTransient<DbConnectionStringBase, DevelopmentDbConnectionString>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbConnectionStringBase dbConnection)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
