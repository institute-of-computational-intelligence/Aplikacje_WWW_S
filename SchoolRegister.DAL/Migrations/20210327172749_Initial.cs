using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolRegister.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "AspNetRoles",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleValue = table.Column<int>("int", nullable: false),
                    Name = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                "Groups",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Groups", x => x.Id); });

            migrationBuilder.CreateTable(
                "AspNetRoleClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>("int", nullable: false),
                    ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUsers",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>("nvarchar(max)", nullable: true),
                    LastName = table.Column<string>("nvarchar(max)", nullable: true),
                    RegisterationDate = table.Column<DateTime>("datetime2", nullable: false),
                    UserType = table.Column<int>("int", nullable: false),
                    GroupId = table.Column<int>("int", nullable: true),
                    ParentId = table.Column<int>("int", nullable: true),
                    Title = table.Column<string>("nvarchar(max)", nullable: true),
                    UserName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>("nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>("bit", nullable: false),
                    PasswordHash = table.Column<string>("nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>("nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>("nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>("nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>("bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>("bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>("datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>("bit", nullable: false),
                    AccessFailedCount = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUsers_AspNetUsers_ParentId",
                        x => x.ParentId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_AspNetUsers_Groups_GroupId",
                        x => x.GroupId,
                        "Groups",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserClaims",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>("int", nullable: false),
                    ClaimType = table.Column<string>("nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        "FK_AspNetUserClaims_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserLogins",
                table => new
                {
                    LoginProvider = table.Column<string>("nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>("nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>("nvarchar(max)", nullable: true),
                    UserId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new {x.LoginProvider, x.ProviderKey});
                    table.ForeignKey(
                        "FK_AspNetUserLogins_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserRoles",
                table => new
                {
                    UserId = table.Column<int>("int", nullable: false),
                    RoleId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new {x.UserId, x.RoleId});
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        x => x.RoleId,
                        "AspNetRoles",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_AspNetUserRoles_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "AspNetUserTokens",
                table => new
                {
                    UserId = table.Column<int>("int", nullable: false),
                    LoginProvider = table.Column<string>("nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>("nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>("nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new {x.UserId, x.LoginProvider, x.Name});
                    table.ForeignKey(
                        "FK_AspNetUserTokens_AspNetUsers_UserId",
                        x => x.UserId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "Subjects",
                table => new
                {
                    Id = table.Column<int>("int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>("nvarchar(max)", nullable: true),
                    Name = table.Column<string>("nvarchar(max)", nullable: true),
                    TeacherId = table.Column<int>("int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        "FK_Subjects_AspNetUsers_TeacherId",
                        x => x.TeacherId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Grades",
                table => new
                {
                    DateOfIssue = table.Column<DateTime>("datetime2", nullable: false),
                    StudentId = table.Column<int>("int", nullable: false),
                    SubjectId = table.Column<int>("int", nullable: false),
                    GradeValue = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => new {x.DateOfIssue, x.SubjectId, x.StudentId});
                    table.ForeignKey(
                        "FK_Grades_AspNetUsers_StudentId",
                        x => x.StudentId,
                        "AspNetUsers",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Grades_Subjects_SubjectId",
                        x => x.SubjectId,
                        "Subjects",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                "SubjectGroups",
                table => new
                {
                    GroupId = table.Column<int>("int", nullable: false),
                    SubjectId = table.Column<int>("int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectGroups", x => new {x.GroupId, x.SubjectId});
                    table.ForeignKey(
                        "FK_SubjectGroups_Groups_GroupId",
                        x => x.GroupId,
                        "Groups",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_SubjectGroups_Subjects_SubjectId",
                        x => x.SubjectId,
                        "Subjects",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_AspNetRoleClaims_RoleId",
                "AspNetRoleClaims",
                "RoleId");

            migrationBuilder.CreateIndex(
                "RoleNameIndex",
                "AspNetRoles",
                "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserClaims_UserId",
                "AspNetUserClaims",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserLogins_UserId",
                "AspNetUserLogins",
                "UserId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUserRoles_RoleId",
                "AspNetUserRoles",
                "RoleId");

            migrationBuilder.CreateIndex(
                "EmailIndex",
                "AspNetUsers",
                "NormalizedEmail");

            migrationBuilder.CreateIndex(
                "IX_AspNetUsers_GroupId",
                "AspNetUsers",
                "GroupId");

            migrationBuilder.CreateIndex(
                "IX_AspNetUsers_ParentId",
                "AspNetUsers",
                "ParentId");

            migrationBuilder.CreateIndex(
                "UserNameIndex",
                "AspNetUsers",
                "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                "IX_Grades_StudentId",
                "Grades",
                "StudentId");

            migrationBuilder.CreateIndex(
                "IX_Grades_SubjectId",
                "Grades",
                "SubjectId");

            migrationBuilder.CreateIndex(
                "IX_SubjectGroups_SubjectId",
                "SubjectGroups",
                "SubjectId");

            migrationBuilder.CreateIndex(
                "IX_Subjects_TeacherId",
                "Subjects",
                "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "AspNetRoleClaims");

            migrationBuilder.DropTable(
                "AspNetUserClaims");

            migrationBuilder.DropTable(
                "AspNetUserLogins");

            migrationBuilder.DropTable(
                "AspNetUserRoles");

            migrationBuilder.DropTable(
                "AspNetUserTokens");

            migrationBuilder.DropTable(
                "Grades");

            migrationBuilder.DropTable(
                "SubjectGroups");

            migrationBuilder.DropTable(
                "AspNetRoles");

            migrationBuilder.DropTable(
                "Subjects");

            migrationBuilder.DropTable(
                "AspNetUsers");

            migrationBuilder.DropTable(
                "Groups");
        }
    }
}