using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_JobType_In_Periodical_allowance_29July2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Payroll_PeriodicallyVariableAllowanceDetail");
        }
    }
}
