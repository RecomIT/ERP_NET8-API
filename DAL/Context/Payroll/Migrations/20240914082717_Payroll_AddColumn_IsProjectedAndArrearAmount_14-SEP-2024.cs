using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_IsProjectedAndArrearAmount_14SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ArrearAmount",
                table: "Payroll_FinalTaxProcessDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProjected",
                table: "Payroll_FinalTaxProcessDetail",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ArrearAmount",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsProjected",
                table: "Payroll_EmployeeTaxProcessDetail",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArrearAmount",
                table: "Payroll_FinalTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "IsProjected",
                table: "Payroll_FinalTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "ArrearAmount",
                table: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropColumn(
                name: "IsProjected",
                table: "Payroll_EmployeeTaxProcessDetail");
        }
    }
}
