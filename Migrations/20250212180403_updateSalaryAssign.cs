using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class updateSalaryAssign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HouseRent",
                table: "SalaryAssign",
                newName: "TotalDeductions");

            migrationBuilder.AddColumn<decimal>(
                name: "AdvanceSalary",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CarAllowance",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CcCharge",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LastMonthLoanPayment",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LoanRepayment",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LunchDeduction",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetSalary",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PF",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "SalaryAssignId",
                table: "SalaryAssign",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvanceSalary",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "CarAllowance",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "CcCharge",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "LastMonthLoanPayment",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "LoanRepayment",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "LunchDeduction",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "NetSalary",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "PF",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "SalaryAssignId",
                table: "SalaryAssign");

            migrationBuilder.RenameColumn(
                name: "TotalDeductions",
                table: "SalaryAssign",
                newName: "HouseRent");
        }
    }
}
