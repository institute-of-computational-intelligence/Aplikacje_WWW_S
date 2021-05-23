using Microsoft.EntityFrameworkCore.Migrations;

namespace SchoolRegister.DAL.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegisterationDate",
                table: "AspNetUsers",
                newName: "RegistrationDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "AspNetUsers",
                newName: "RegisterationDate");
        }
    }
}
