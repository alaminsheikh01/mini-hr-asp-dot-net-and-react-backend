using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class updateEmpSalaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BasicSalary",
                table: "EmployeeSalary",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Conveyance",
                table: "EmployeeSalary",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HouseRent",
                table: "EmployeeSalary",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MedicalAllowance",
                table: "EmployeeSalary",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasicSalary",
                table: "EmployeeSalary");

            migrationBuilder.DropColumn(
                name: "Conveyance",
                table: "EmployeeSalary");

            migrationBuilder.DropColumn(
                name: "HouseRent",
                table: "EmployeeSalary");

            migrationBuilder.DropColumn(
                name: "MedicalAllowance",
                table: "EmployeeSalary");
        }
    }
}
