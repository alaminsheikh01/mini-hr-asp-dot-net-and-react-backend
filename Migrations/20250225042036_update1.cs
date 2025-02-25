using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Employee",
                newName: "DesignationName");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "DesignationName",
                table: "Employee",
                newName: "City");
        }
    }
}
