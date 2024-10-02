using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_DefaultInvestmentAndRebate_10SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DefaultInvestment",
                table: "Payroll_FinalTaxProcess",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultRebate",
                table: "Payroll_FinalTaxProcess",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultInvestment",
                table: "Payroll_EmployeeTaxProcess",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DefaultRebate",
                table: "Payroll_EmployeeTaxProcess",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultInvestment",
                table: "Payroll_FinalTaxProcess");

            migrationBuilder.DropColumn(
                name: "DefaultRebate",
                table: "Payroll_FinalTaxProcess");

            migrationBuilder.DropColumn(
                name: "DefaultInvestment",
                table: "Payroll_EmployeeTaxProcess");

            migrationBuilder.DropColumn(
                name: "DefaultRebate",
                table: "Payroll_EmployeeTaxProcess");
        }
    }
}
