using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_UpdateColumn_In_PeriodicalAllowanceDetail_29July2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllownanceNameId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "AllowanceNameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllowanceNameId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                newName: "AllownanceNameId");
        }
    }
}
