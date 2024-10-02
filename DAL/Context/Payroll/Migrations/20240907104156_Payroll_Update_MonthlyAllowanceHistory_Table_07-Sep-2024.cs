using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_Update_MonthlyAllowanceHistory_Table_07Sep2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payroll_MonthlyAllowanceHistory_Payroll_MonthlyAllowanceConfig_MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory");

            migrationBuilder.DropIndex(
                name: "IX_Payroll_MonthlyAllowanceHistory_MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory");

            migrationBuilder.DropColumn(
                name: "MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory");

            migrationBuilder.RenameColumn(
                name: "MonthlyAllowanceConfig",
                table: "Payroll_MonthlyAllowanceHistory",
                newName: "MonthlyAllowanceConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAllowanceHistory_MonthlyAllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory",
                column: "MonthlyAllowanceConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payroll_MonthlyAllowanceHistory_Payroll_MonthlyAllowanceConfig_MonthlyAllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory",
                column: "MonthlyAllowanceConfigId",
                principalTable: "Payroll_MonthlyAllowanceConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payroll_MonthlyAllowanceHistory_Payroll_MonthlyAllowanceConfig_MonthlyAllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory");

            migrationBuilder.DropIndex(
                name: "IX_Payroll_MonthlyAllowanceHistory_MonthlyAllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory");

            migrationBuilder.RenameColumn(
                name: "MonthlyAllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory",
                newName: "MonthlyAllowanceConfig");

            migrationBuilder.AddColumn<long>(
                name: "MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAllowanceHistory_MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory",
                column: "MonthlyAlllowanceConfigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payroll_MonthlyAllowanceHistory_Payroll_MonthlyAllowanceConfig_MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceHistory",
                column: "MonthlyAlllowanceConfigId",
                principalTable: "Payroll_MonthlyAllowanceConfig",
                principalColumn: "Id");
        }
    }
}
