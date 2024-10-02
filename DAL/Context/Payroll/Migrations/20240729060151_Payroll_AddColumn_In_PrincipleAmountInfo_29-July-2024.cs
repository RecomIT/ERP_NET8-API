using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_In_PrincipleAmountInfo_29July2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "Payroll_PrincipleAmountInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DesignationId",
                table: "Payroll_PrincipleAmountInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "Payroll_PrincipleAmountInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "GradeId",
                table: "Payroll_PrincipleAmountInfo",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                table: "Payroll_PrincipleAmountInfo",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReplaceId",
                table: "Payroll_PrincipleAmountInfo",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Payroll_PrincipleAmountInfo");

            migrationBuilder.DropColumn(
                name: "DesignationId",
                table: "Payroll_PrincipleAmountInfo");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Payroll_PrincipleAmountInfo");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Payroll_PrincipleAmountInfo");

            migrationBuilder.DropColumn(
                name: "JobType",
                table: "Payroll_PrincipleAmountInfo");

            migrationBuilder.DropColumn(
                name: "ReplaceId",
                table: "Payroll_PrincipleAmountInfo");
        }
    }
}
