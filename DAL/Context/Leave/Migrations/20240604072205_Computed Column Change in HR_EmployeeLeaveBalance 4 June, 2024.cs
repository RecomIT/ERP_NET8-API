using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Leave.Migrations
{
    /// <inheritdoc />
    public partial class ComputedColumnChangeinHR_EmployeeLeaveBalance4June2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HR_EmployeeLeaveBalance",
                table: "HR_EmployeeLeaveBalance");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HR_EmployeeLeaveBalance",
                table: "HR_EmployeeLeaveBalance",
                column: "EmployeeLeaveBalanceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_HR_EmployeeLeaveBalance",
                table: "HR_EmployeeLeaveBalance");

            migrationBuilder.AddPrimaryKey(
                name: "PK_HR_EmployeeLeaveBalance",
                table: "HR_EmployeeLeaveBalance",
                columns: new[] { "EmployeeLeaveBalanceId", "EmployeeId", "LeaveTypeId", "LeaveYear" });
        }
    }
}
