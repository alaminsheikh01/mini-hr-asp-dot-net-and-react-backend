using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class addedThreeValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FestivalBonus",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IncomeTax",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PerformanceBonus",
                table: "SalaryAssign",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FestivalBonus",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "IncomeTax",
                table: "SalaryAssign");

            migrationBuilder.DropColumn(
                name: "PerformanceBonus",
                table: "SalaryAssign");
        }
    }
}
