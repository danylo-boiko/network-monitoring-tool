using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Nmt.Infrastructure.Data.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true),
                    Discriminator = table.Column<string>(type: "text", nullable: false)
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
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Hostname = table.Column<string>(type: "text", nullable: false),
                    MachineSpecificStamp = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IpFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Ip = table.Column<long>(type: "bigint", nullable: false),
                    FilterAction = table.Column<byte>(type: "smallint", nullable: false),
                    Comment = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IpFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IpFilters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9"), null, "User", "USER" },
                    { new Guid("3078ee96-5674-432b-9493-f16fab2f6dab"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "Discriminator", "RoleId" },
                values: new object[,]
                {
                    { 22, "Permission", "1", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 23, "Permission", "2", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 24, "Permission", "3", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 25, "Permission", "5", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 26, "Permission", "6", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 27, "Permission", "9", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 28, "Permission", "10", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 29, "Permission", "11", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 30, "Permission", "12", "RoleClaim", new Guid("26528216-9c53-4c9f-bd6b-22ec59c6f3c9") },
                    { 31, "Permission", "1", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 32, "Permission", "2", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 33, "Permission", "3", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 34, "Permission", "4", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 35, "Permission", "5", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 36, "Permission", "6", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 37, "Permission", "7", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 38, "Permission", "8", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 39, "Permission", "9", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 40, "Permission", "10", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 41, "Permission", "11", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") },
                    { 42, "Permission", "12", "RoleClaim", new Guid("3078ee96-5674-432b-9493-f16fab2f6dab") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

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
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId_MachineSpecificStamp",
                table: "Devices",
                columns: new[] { "UserId", "MachineSpecificStamp" });

            migrationBuilder.CreateIndex(
                name: "IX_IpFilters_UserId_Ip",
                table: "IpFilters",
                columns: new[] { "UserId", "Ip" },
                unique: true);
        }

        /// <inheritdoc />
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
                name: "Devices");

            migrationBuilder.DropTable(
                name: "IpFilters");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
