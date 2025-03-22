using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class addedPayscaleSetupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AcademicQualifications",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BloodGroup",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CertificateVerification",
                table: "Employee",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmationDate",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DisciplinaryAction",
                table: "Employee",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ETIN",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmergencyContact",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeSalaryGrade",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeType",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobLocation",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NID",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PermanentAddress",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PoliceVerification",
                table: "Employee",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PresentAddress",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RetirementOrResignation",
                table: "Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalaryAccountNumber",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServicePeriod",
                table: "Employee",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicQualifications",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "BloodGroup",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CertificateVerification",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ConfirmationDate",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "DisciplinaryAction",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ETIN",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmergencyContact",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeSalaryGrade",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeType",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "JobLocation",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "NID",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PermanentAddress",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PoliceVerification",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "PresentAddress",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "RetirementOrResignation",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "SalaryAccountNumber",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "ServicePeriod",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employee");
        }
    }
}
