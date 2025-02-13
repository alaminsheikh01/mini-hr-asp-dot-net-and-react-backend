using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class updateforId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseRent",
                table: "EmployeeSalary");

            migrationBuilder.AddColumn<int>(
                name: "SalaryAssignId",
                table: "EmployeeSalary",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaryAssignId",
                table: "EmployeeSalary");

            migrationBuilder.AddColumn<decimal>(
                name: "HouseRent",
                table: "EmployeeSalary",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
