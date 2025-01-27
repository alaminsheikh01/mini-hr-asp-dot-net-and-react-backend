﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstOne.Migrations
{
    public partial class loanCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Loan");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Loan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    Installment = table.Column<int>(type: "int", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoanAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoanDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LoanType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loan", x => x.Id);
                });
        }
    }
}
