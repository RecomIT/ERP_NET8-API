using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_PayableMonthAndYear_ConditionalProjection_29SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "PayableMonth",
                table: "Payroll_ConditionalProjectedPayment",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PayableYear",
                table: "Payroll_ConditionalProjectedPayment",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayableMonth",
                table: "Payroll_ConditionalProjectedPayment");

            migrationBuilder.DropColumn(
                name: "PayableYear",
                table: "Payroll_ConditionalProjectedPayment");
        }
    }
}
