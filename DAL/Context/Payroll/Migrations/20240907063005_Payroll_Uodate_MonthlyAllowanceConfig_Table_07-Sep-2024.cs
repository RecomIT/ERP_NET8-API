using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_Uodate_MonthlyAllowanceConfig_Table_07Sep2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payroll_MonthlyAllowanceConfigParameter");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyAlllowanceConfig");

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyAllowanceConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsProrated = table.Column<bool>(type: "bit", nullable: false),
                    IsVisibleInSalarySheet = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ActivationFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ActivationTo = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_MonthlyAllowanceConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyAllowanceHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Days = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    MonthlyAllowanceConfig = table.Column<long>(type: "bigint", nullable: false),
                    MonthlyAlllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_MonthlyAllowanceHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_MonthlyAllowanceHistory_Payroll_MonthlyAllowanceConfig_MonthlyAlllowanceConfigId",
                        column: x => x.MonthlyAlllowanceConfigId,
                        principalTable: "Payroll_MonthlyAllowanceConfig",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAllowanceConfig_NonClusteredIndex",
                table: "Payroll_MonthlyAllowanceConfig",
                columns: new[] { "EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAllowanceHistory_MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory",
                column: "MonthlyAlllowanceConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payroll_MonthlyAllowanceHistory");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyAllowanceConfig");

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyAlllowanceConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivationFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ActivationTo = table.Column<DateTime>(type: "date", nullable: true),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    Citizen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HasBranch = table.Column<bool>(type: "bit", nullable: true),
                    HasDepartment = table.Column<bool>(type: "bit", nullable: true),
                    HasDesignation = table.Column<bool>(type: "bit", nullable: true),
                    HasDivision = table.Column<bool>(type: "bit", nullable: true),
                    HasEmployee = table.Column<bool>(type: "bit", nullable: true),
                    HasEmployeeTypes = table.Column<bool>(type: "bit", nullable: true),
                    HasExcludeEmployee = table.Column<bool>(type: "bit", nullable: true),
                    HasGrade = table.Column<bool>(type: "bit", nullable: true),
                    HasInternalDesignation = table.Column<bool>(type: "bit", nullable: true),
                    HasSection = table.Column<bool>(type: "bit", nullable: true),
                    HasSubSection = table.Column<bool>(type: "bit", nullable: true),
                    HasUnit = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    PFEmployee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PhysicalCondition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ServiceLength = table.Column<int>(type: "int", nullable: true),
                    ServiceLengthUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_MonthlyAlllowanceConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyAllowanceConfigParameter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthlyAlllowanceConfigId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    ParameterId = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_MonthlyAllowanceConfigParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_MonthlyAllowanceConfigParameter_Payroll_MonthlyAlllowanceConfig_MonthlyAlllowanceConfigId",
                        column: x => x.MonthlyAlllowanceConfigId,
                        principalTable: "Payroll_MonthlyAlllowanceConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAlllowanceConfig_NonClusteredIndex",
                table: "Payroll_MonthlyAlllowanceConfig",
                columns: new[] { "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAllowanceConfigParameter_MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceConfigParameter",
                column: "MonthlyAlllowanceConfigId");
        }
    }
}
