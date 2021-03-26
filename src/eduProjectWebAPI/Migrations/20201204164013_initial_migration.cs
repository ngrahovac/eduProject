﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eduProjectWebAPI.Migrations
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "255fd95b-c771-4be6-852c-5634244b17ac", "nikolinagrahovac@test.com", true, true, null, "NIKOLINAGRAHOVAC@TEST.COM", "NIKOLINAGRAHOVAC@TEST.COM", "AQAAAAEAACcQAAAAEB3fErhGcr/yNJzrnpwkMN0eSAjxvNKxRhnu+pc/nKkNmCuBIFm9Hb6ow4nuD2EBrA==", null, false, "BRPZ5G5VMPU533RX5YC3S62EYN5H22EL", false, "nikolinagrahovac@test.com" },
                    { "2", 0, "80088041-9c29-474a-8668-3277462c4d51", "zoranpantos@test.com", true, true, null, "ZORANPANTOS@TEST.COM", "ZORANPANTOS@TEST.COM", "AQAAAAEAACcQAAAAEFhPDeoQp9iRyCWYDdk+1+BVvrNObJl/WW/vJBiezvzhe69X92GG4+z9XUzHS8qN3A==", null, false, "7ZTQGH5Q7AX4CPI73DWSQS2OMESYR6KT", false, "zoranpantos@test.com" },
                    { "3", 0, "1dd40535-18b6-47ec-bb00-d0b770505ad1", "nikolinamaksimovic@test.com", true, true, null, "NIKOLINAMAKSIMOVIC@TEST.COM", "NIKOLINAMAKSIMOVIC@TEST.COM", "AQAAAAEAACcQAAAAEKNjZLA00+meOz9bw9rokp6svoTGUBUu2psELguBARlsD8aQ2DULpNZwMgnvfv4amw==", null, false, "ZW5I3YK2S6L6WZCX3WNE24LITMDVRXA5", false, "nikolinamaksimovic@test.com" },
                    { "4", 0, "f8e8dfc8-f20e-44fe-9d46-2e322c1d41df", "branislavkljajic@test.com", true, true, null, "BRANISLAVKLJAJIC@TEST.COM", "BRANISLAVKLJAJIC@TEST.COM", "AQAAAAEAACcQAAAAEKNDYYoI7EzH7+XXPzTmscNqBXX0sM8WqCejUG2K9Uj0vGPHrMzlghVlhiA1Zx5iLA==", null, false, "7XZHWB2VVO2IAE6OLBPGLASCGPBD7GVD", false, "branislavkljajic@test.com" },
                    { "5", 0, "279ebb84-8ebf-461d-96bb-01a36e1e4604", "markomarkovic@test.com", true, true, null, "MARKOMARKOVIC@TEST.COM", "MARKOMARKOVIC@TEST.COM", "AQAAAAEAACcQAAAAEHSUcUGJ80NRBA51IFIT6Hmdsnnd2X6K+88Qh32WXVGDGyp0yGq7oD7KFzQEXPCYXw==", null, false, "YCRUMPJQ4XDVK5SVH4NKKCZD7GT6L2SJ", false, "markomarkovic@test.com" },
                    { "6", 0, "f7a176dd-d392-4e72-b558-df9f884fbf16", "petrapetric@test.com", true, true, null, "PETRAPETRIC@TEST.COM", "PETRAPETRIC@TEST.COM", "AQAAAAEAACcQAAAAEFjzJQwLfa8qiBv+vkRPAVb1LkSqhIpria+0GAb4dTFf6ZFvl+3H4Z5SGhvYRKrQ/g==", null, false, "ECDDLUTS3UCL2SJJUPKBZS7IFNPX4BFH", false, "petrapetric@test.com" },
                    { "7", 0, "f4de8181-2f1d-41fa-9f53-31f69b7d79d2", "milanmilanic@test.com", true, true, null, "MILANMILANIC@TEST.COM", "MILANMILANIC@TEST.COM", "AQAAAAEAACcQAAAAEIMbanQNVgP7HbGL2D8KdCCe4Ika8BlGvzCHO2WrF1Yl2nEFzrrJLJ6a6D+9WbqbHA==", null, false, "MIUGYEXVPHTXYDIJ2ZPSHCEJFCO2WZVI", false, "milanmilanic@test.com" },
                    { "8", 0, "1f0ddbb3-a6c1-4a91-9e54-a1066b9dca42", "ananic@test.com", true, true, null, "ANAANIC@TEST.COM", "ANAANIC@TEST.COM", "AQAAAAEAACcQAAAAEEEVprGGdlzE/trKNkFDEoADsUgO0dy16A6e2zleelYn1IwAhDAQTRluj+BkldeajQ==", null, false, "4RONA2UUHXSB2F626TUHKHMP24PND6QA", false, "anaanic@test.com" },
                    { "9", 0, "43c22374-97c4-4278-9cde-bc4713f26d3d", "admin@test.com", true, true, null, "ADMIN@TEST.COM", "ADMIN@TEST.COM", "AQAAAAEAACcQAAAAEBdVV+vJRq7/JT6OxpmfJaoy8PUReiGDs63Ff8M6FdHGv6fyXpgCh6SLRrwu4bn1xA==", null, false, "JZV6LUEYDIMPJ6SXZGEUMGEWKRLAENAI", false, "admin@test.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { "1", "Admin", "ADMIN", "bab4afd0-59bf-498e-8712-5ef8dcc78103" }
                }
                );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "UserId", "RoleId" },
                values: new object[,]
                {
                    { "9", "1" }
                }
                );

            migrationBuilder.AddColumn<bool>(
                name: "ActiveStatus",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: true
                );

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
