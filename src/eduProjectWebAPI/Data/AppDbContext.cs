using eduProjectModel.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eduProjectWebAPI.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            /*Passwords: Nina - nikolina, Maxa - nikolina, Bane - branislav, Zoran - zoran*/

            builder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                   Id = "1",
                   UserName = "nikolinagrahovac@test.com",
                   NormalizedUserName = "NIKOLINAGRAHOVAC@TEST.COM",
                   Email = "nikolinagrahovac@test.com",
                   NormalizedEmail = "NIKOLINAGRAHOVAC@TEST.COM",
                   EmailConfirmed = false,
                   PasswordHash = "AQAAAAEAACcQAAAAEB3fErhGcr/yNJzrnpwkMN0eSAjxvNKxRhnu+pc/nKkNmCuBIFm9Hb6ow4nuD2EBrA==",
                   SecurityStamp = "BRPZ5G5VMPU533RX5YC3S62EYN5H22EL",
                   ConcurrencyStamp = "255fd95b-c771-4be6-852c-5634244b17ac",
                   PhoneNumberConfirmed = false,
                   TwoFactorEnabled = false,
                   LockoutEnabled = true
                },
                new ApplicationUser
                {
                    Id = "2",
                    UserName = "nikolinamaksimovic@test.com",
                    NormalizedUserName = "NIKOLINAMAKSIMOVIC@TEST.COM",
                    Email = "nikolinamaksimovic@test.com",
                    NormalizedEmail = "NIKOLINAMAKSIMOVIC@TEST.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEKNjZLA00+meOz9bw9rokp6svoTGUBUu2psELguBARlsD8aQ2DULpNZwMgnvfv4amw==",
                    SecurityStamp = "ZW5I3YK2S6L6WZCX3WNE24LITMDVRXA5",
                    ConcurrencyStamp = "1dd40535-18b6-47ec-bb00-d0b770505ad1",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true
                },
                new ApplicationUser
                {
                    Id = "3",
                    UserName = "branislavkljajic@test.com",
                    NormalizedUserName = "BRANISLAVKLJAJIC@TEST.COM",
                    Email = "branislavkljajic@test.com",
                    NormalizedEmail = "BRANISLAVKLJAJIC@TEST.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEKNDYYoI7EzH7+XXPzTmscNqBXX0sM8WqCejUG2K9Uj0vGPHrMzlghVlhiA1Zx5iLA==",
                    SecurityStamp = "7XZHWB2VVO2IAE6OLBPGLASCGPBD7GVD",
                    ConcurrencyStamp = "f8e8dfc8-f20e-44fe-9d46-2e322c1d41df",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true
                },
                new ApplicationUser
                {
                    Id = "4",
                    UserName = "zoranpantos@test.com",
                    NormalizedUserName = "ZORANPANTOS@TEST.COM",
                    Email = "zoranpantos@test.com",
                    NormalizedEmail = "ZORANPANTOS@TEST.COM",
                    EmailConfirmed = false,
                    PasswordHash = "AQAAAAEAACcQAAAAEFhPDeoQp9iRyCWYDdk+1+BVvrNObJl/WW/vJBiezvzhe69X92GG4+z9XUzHS8qN3A==",
                    SecurityStamp = "7ZTQGH5Q7AX4CPI73DWSQS2OMESYR6KT",
                    ConcurrencyStamp = "80088041-9c29-474a-8668-3277462c4d51",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true
                }
                );
        }
    }
}
