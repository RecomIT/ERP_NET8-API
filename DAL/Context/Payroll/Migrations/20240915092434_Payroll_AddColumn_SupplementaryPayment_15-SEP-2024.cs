using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_SupplementaryPayment_15SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotaTax",
                table: "Payroll_SupplementaryPaymentProcessInfo",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxAmount",
                table: "Payroll_SupplementaryPaymentAmount",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotaTax",
                table: "Payroll_SupplementaryPaymentProcessInfo");

            migrationBuilder.DropColumn(
                name: "TaxAmount",
                table: "Payroll_SupplementaryPaymentAmount");
        }
    }
}
