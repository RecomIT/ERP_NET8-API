using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Employee.Migrations
{
    /// <inheritdoc />
    public partial class HR_Add_Lunch_Request : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_LunchRate",
                columns: table => new
                {
                    LunchRateId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ValidFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ValidTo = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LunchRate", x => x.LunchRateId);
                });

            migrationBuilder.CreateTable(
                name: "HR_LunchRequests",
                columns: table => new
                {
                    LunchRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsLunch = table.Column<bool>(type: "bit", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "date", nullable: false),
                    GuestCount = table.Column<int>(type: "int", nullable: true),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: true),
                    RequestedByAdminId = table.Column<long>(type: "bigint", nullable: true),
                    LunchRateId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LunchRequests", x => x.LunchRequestId);
                    table.ForeignKey(
                        name: "FK_HR_LunchRequests_HR_LunchRate_LunchRateId",
                        column: x => x.LunchRateId,
                        principalTable: "HR_LunchRate",
                        principalColumn: "LunchRateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_LunchRate_NonClusteredIndex",
                table: "HR_LunchRate",
                columns: new[] { "LunchRateId", "Rate", "ValidFrom", "ValidTo", "CompanyId", "OrganizationId", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_LunchRequests_LunchRateId",
                table: "HR_LunchRequests",
                column: "LunchRateId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_LunchRequests_NonClusteredIndex",
                table: "HR_LunchRequests",
                columns: new[] { "LunchRequestId", "EmployeeId", "LunchRateId", "RequestDate", "IsLunch", "IsCanceled", "RequestedOn", "RequestedByAdminId", "CompanyId", "OrganizationId", "BranchId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_LunchRequests");

            migrationBuilder.DropTable(
                name: "HR_LunchRate");
        }
    }
}
