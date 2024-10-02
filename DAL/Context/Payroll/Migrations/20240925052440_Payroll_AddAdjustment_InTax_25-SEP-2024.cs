using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddAdjustment_InTax_25SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalAdjustmentAmount",
                table: "Payroll_SupplementaryPaymentTaxInfo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAdjusmentAmount",
                table: "Payroll_SupplementaryPaymentTaxDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TillAdjusmentAmount",
                table: "Payroll_SupplementaryPaymentTaxDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAdjusmentAmount",
                table: "Payroll_FinalTaxProcessDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TillAdjusmentAmount",
                table: "Payroll_FinalTaxProcessDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAdjustmentAmount",
                table: "Payroll_FinalTaxProcess",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentAdjusmentAmount",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TillAdjusmentAmount",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAdjustmentAmount",
                table: "Payroll_EmployeeTaxProcess",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalAdjustmentAmount",
                table: "Payroll_SupplementaryPaymentTaxInfo");

            migrationBuilder.DropColumn(
                name: "CurrentAdjusmentAmount",
                table: "Payroll_SupplementaryPaymentTaxDetail");

            migrationBuilder.DropColumn(
                name: "TillAdjusmentAmount",
                table: "Payroll_SupplementaryPaymentTaxDetail");

            migrationBuilder.DropColumn(
                name: "CurrentAdjusmentAmount",
                table: "Payroll_FinalTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "TillAdjusmentAmount",
                table: "Payroll_FinalTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "TotalAdjustmentAmount",
                table: "Payroll_FinalTaxProcess");

            migrationBuilder.DropColumn(
                name: "CurrentAdjusmentAmount",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "TillAdjusmentAmount",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "TotalAdjustmentAmount",
                table: "Payroll_EmployeeTaxProcess");
        }
    }
}
