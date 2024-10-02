using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddTable_SpecialTaxSlab_13Aug2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payroll_SpecialTaxSlab",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    AssessmentYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlabMininumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SlabMaximumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SlabPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_SpecialTaxSlab", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SpecialTaxSlab_NonClusteredIndex",
                table: "Payroll_SpecialTaxSlab",
                columns: new[] { "EmployeeId", "FiscalYearId", "ImpliedCondition", "StateStatus", "CompanyId", "OrganizationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payroll_SpecialTaxSlab");
        }
    }
}
