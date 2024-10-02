using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_JobType_SalaryAllowanceConfigDetail_21July2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payroll_PeriodicallyVariableAllowanceDetail_Payroll_PeriodicallyVariableAllowanceInfo_PeriodicallyVariableAllowanceInfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail");

            migrationBuilder.RenameColumn(
                name: "PeriodicallyVariableAllowanceInfoId",
                table: "Payroll_PeriodicallyVariableAllowanceInfo",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PeriodicallyVariableAllowanceInfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "InfoId");

            migrationBuilder.RenameColumn(
                name: "PeriodicallyVariableAllowanceDetailId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Payroll_PeriodicallyVariableAllowanceDetail_PeriodicallyVariableAllowanceInfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "IX_Payroll_PeriodicallyVariableAllowanceDetail_InfoId");

            migrationBuilder.AddColumn<long>(
                name: "PaymentAmountId",
                table: "Payroll_SupplementaryPaymentTaxSlab",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PaymentProcessInfoId",
                table: "Payroll_SupplementaryPaymentTaxSlab",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PaymentAmountId",
                table: "Payroll_SupplementaryPaymentTaxDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PaymentProcessInfoId",
                table: "Payroll_SupplementaryPaymentTaxDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "AllowanceNameId",
                table: "Payroll_SupplementaryPaymentProcessInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankBranchId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BankId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CostCenterId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CostCenterName",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DesignationId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeType",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeTypeId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Grade",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GradeId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InternalDesignation",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "InternalDesignationId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobCategory",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "JobCategoryId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SectionId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubSection",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubSectionId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "YearlyCTC",
                table: "Payroll_SalaryReviewInfo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DiscontinuedId",
                table: "Payroll_SalaryHold",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BreakdownName",
                table: "Payroll_SalaryAllowanceConfigurationInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Payroll_SalaryAllowanceConfigurationDetails",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeductionConfigId",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeductionHeadId",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeductionHeadName",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeductionNameId",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeductionNameName",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AdditionalSubstractAmount",
                table: "Payroll_EmployeeTaxProcess",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AllowanceReason",
                table: "Payroll_EmployeeProjectedAllowance",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payroll_PeriodicallyVariableAllowanceDetail_Payroll_PeriodicallyVariableAllowanceInfo_InfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                column: "InfoId",
                principalTable: "Payroll_PeriodicallyVariableAllowanceInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payroll_PeriodicallyVariableAllowanceDetail_Payroll_PeriodicallyVariableAllowanceInfo_InfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail");

            migrationBuilder.DropColumn(
                name: "PaymentAmountId",
                table: "Payroll_SupplementaryPaymentTaxSlab");

            migrationBuilder.DropColumn(
                name: "PaymentProcessInfoId",
                table: "Payroll_SupplementaryPaymentTaxSlab");

            migrationBuilder.DropColumn(
                name: "PaymentAmountId",
                table: "Payroll_SupplementaryPaymentTaxDetail");

            migrationBuilder.DropColumn(
                name: "PaymentProcessInfoId",
                table: "Payroll_SupplementaryPaymentTaxDetail");

            migrationBuilder.DropColumn(
                name: "AllowanceNameId",
                table: "Payroll_SupplementaryPaymentProcessInfo");

            migrationBuilder.DropColumn(
                name: "BankBranchId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "CostCenterName",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "EmployeeType",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "EmployeeTypeId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "Grade",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "InternalDesignation",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "InternalDesignationId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "JobCategory",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "JobCategoryId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "SubSection",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "SubSectionId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropColumn(
                name: "YearlyCTC",
                table: "Payroll_SalaryReviewInfo");

            migrationBuilder.DropColumn(
                name: "DiscontinuedId",
                table: "Payroll_SalaryHold");

            migrationBuilder.DropColumn(
                name: "BreakdownName",
                table: "Payroll_SalaryAllowanceConfigurationInfo");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Payroll_SalaryAllowanceConfigurationDetails");

            migrationBuilder.DropColumn(
                name: "DeductionConfigId",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "DeductionHeadId",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "DeductionHeadName",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "DeductionNameId",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "DeductionNameName",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "AdditionalSubstractAmount",
                table: "Payroll_EmployeeTaxProcess");

            migrationBuilder.DropColumn(
                name: "AllowanceReason",
                table: "Payroll_EmployeeProjectedAllowance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payroll_PeriodicallyVariableAllowanceInfo",
                newName: "PeriodicallyVariableAllowanceInfoId");

            migrationBuilder.RenameColumn(
                name: "InfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "PeriodicallyVariableAllowanceInfoId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "PeriodicallyVariableAllowanceDetailId");

            migrationBuilder.RenameIndex(
                name: "IX_Payroll_PeriodicallyVariableAllowanceDetail_InfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "IX_Payroll_PeriodicallyVariableAllowanceDetail_PeriodicallyVariableAllowanceInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payroll_PeriodicallyVariableAllowanceDetail_Payroll_PeriodicallyVariableAllowanceInfo_PeriodicallyVariableAllowanceInfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                column: "PeriodicallyVariableAllowanceInfoId",
                principalTable: "Payroll_PeriodicallyVariableAllowanceInfo",
                principalColumn: "PeriodicallyVariableAllowanceInfoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
