using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class addedPayscaleSetupTable1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PayScaleSetup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeSalaryGrade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HouseRent = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MedicalAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Conveyance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CarAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DriversSalaryReimbursement = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DelFlag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayScaleSetup", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayScaleSetup");
        }
    }
}
