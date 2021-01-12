using eduProjectModel.Domain;
using eduProjectWebAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

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

            services.AddCors();

            services.AddControllers();

            services.AddMemoryCache();

            ConfigureDataLayerServices(services);

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("EduProjectIdentityDB")));

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            });

            //Global API authorization (equal to having an [Authorize] attribute above every controller class)
            /*services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });*/

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "70290035778-q5ujcqm43l9mt53be4cma09jsimtniva.apps.googleusercontent.com";
                options.ClientSecret = "ZFKtPq_liv_LtL5xTlXJ4wg_";
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtIssuer"],
                    ValidAudience = configuration["JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecurityKey"]))
                };
            });
        }

        private void ConfigureDataLayerServices(IServiceCollection services)
        {
            services.AddTransient<IProjectsRepository, ProjectsRepository>();
            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IFacultiesRepository, FacultiesRepository>();
            services.AddTransient<IProjectApplicationsRepository, ProjectApplicationsRepository>();
            services.AddTransient<IUserSettingsRepository, UserSettingsRepository>();
            services.AddTransient<DbConnectionParameters, DevelopmentDbConnectionParameters>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DbConnectionParameters dbConnection)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

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
