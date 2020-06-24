using System;
using eduProjectWebAPI.Data.DAO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace eduProjectWebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // from https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql
            services.AddDbContextPool<EduProjectDbContext>(options => options
                .UseMySql(Configuration["ConnectionStrings:eduProjectDb"], mySqlOptions => mySqlOptions
                    .ServerVersion(new Version(5, 6, 40), ServerType.MySql)
            ));

            // register DAO interfaces for injecting into controller ctors
            services.AddScoped<IProjectDAO, ProjectDAOMySQL>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
