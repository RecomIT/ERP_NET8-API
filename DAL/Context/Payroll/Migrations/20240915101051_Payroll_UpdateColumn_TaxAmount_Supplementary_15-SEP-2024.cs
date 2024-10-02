using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_UpdateColumn_TaxAmount_Supplementary_15SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotaTax",
                table: "Payroll_SupplementaryPaymentProcessInfo",
                newName: "TotalTax");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalTax",
                table: "Payroll_SupplementaryPaymentProcessInfo",
                newName: "TotaTax");
        }
    }
}
