using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddTable_FinalTaxProcess_10SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payroll_FinalTaxProcess",
                columns: table => new
                {
                    TaxProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    RemainMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    TotalTillMonthAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCurrentMonthAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalProjectedAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLessExemptedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalGrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrossTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptionAmountOnAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFContributionBothPart = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalTaxPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvestmentRebateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualInvestmentMade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AITAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxReturnAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExcessTaxPaidRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidTotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplementaryOnceffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFExemption = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalIncomeAfterPFExemption = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxProcessUniqId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActualTaxDeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentMonthProjectedTaxDeducted = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentMonthOnceOffTaxDeducted = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YTDLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectionLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TillProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TillOnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalSubstractAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AcceptedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptorRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_FinalTaxProcess", x => x.TaxProcessId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_FinalTaxProcessDetail",
                columns: table => new
                {
                    TaxProcessDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxProcessId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BonusId = table.Column<long>(type: "bigint", nullable: false),
                    BonusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxItem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TillDateIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentMonthIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectedIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessExempted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPerquisite = table.Column<bool>(type: "bit", nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    AllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
                    DeductionHeadId = table.Column<long>(type: "bigint", nullable: true),
                    DeductionHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: true),
                    DeductionNameName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DeductionConfigId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_FinalTaxProcessDetail", x => x.TaxProcessDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_FinalTaxProcessSlab",
                columns: table => new
                {
                    EmployeeTaxProcessSlabId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    IncomeTaxSlabId = table.Column<long>(type: "bigint", nullable: true),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlabPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxLiability = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaxProcessId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_FinalTaxProcessSlab", x => x.EmployeeTaxProcessSlabId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_FinalTaxProcess_NonClusteredIndex",
                table: "Payroll_FinalTaxProcess",
                columns: new[] { "EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_FinalTaxProcessDetail_NonClusteredIndex",
                table: "Payroll_FinalTaxProcessDetail",
                columns: new[] { "EmployeeId", "AllowanceNameId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_FinalTaxProcessSlab_NonClusteredIndex",
                table: "Payroll_FinalTaxProcessSlab",
                columns: new[] { "EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payroll_FinalTaxProcess");

            migrationBuilder.DropTable(
                name: "Payroll_FinalTaxProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_FinalTaxProcessSlab");
        }
    }
}
