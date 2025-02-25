using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class addedSalaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SalaryCode",
                table: "EmployeeSalary",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalaryHeaderId",
                table: "EmployeeSalary",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SalaryHeader",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Month = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Year = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DesignationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryHeader", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeSalary_SalaryHeaderId",
                table: "EmployeeSalary",
                column: "SalaryHeaderId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeSalary_SalaryHeader_SalaryHeaderId",
                table: "EmployeeSalary",
                column: "SalaryHeaderId",
                principalTable: "SalaryHeader",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeSalary_SalaryHeader_SalaryHeaderId",
                table: "EmployeeSalary");

            migrationBuilder.DropTable(
                name: "SalaryHeader");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeSalary_SalaryHeaderId",
                table: "EmployeeSalary");

            migrationBuilder.DropColumn(
                name: "SalaryCode",
                table: "EmployeeSalary");

            migrationBuilder.DropColumn(
                name: "SalaryHeaderId",
                table: "EmployeeSalary");
        }
    }
}
