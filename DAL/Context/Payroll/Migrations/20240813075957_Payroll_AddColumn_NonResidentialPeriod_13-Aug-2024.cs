using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_NonResidentialPeriod_13Aug2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "NonResidentialPeriod",
                table: "Payroll_IncomeTaxSetting",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryComponentHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComponentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_SalaryComponentHistory", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payroll_SalaryComponentHistory");

            migrationBuilder.DropColumn(
                name: "NonResidentialPeriod",
                table: "Payroll_IncomeTaxSetting");
        }
    }
}
