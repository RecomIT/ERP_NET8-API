using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Overtime.Migrations
{
    /// <inheritdoc />
    public partial class OvertimeModuleInit19SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeApprovalLevel",
                columns: table => new
                {
                    OvertimeApprovalLevelId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaximumLevel = table.Column<int>(type: "int", nullable: false),
                    MinimumLevel = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_OvertimeApprovalLevel", x => x.OvertimeApprovalLevelId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeApprover",
                columns: table => new
                {
                    OvertimeApproverId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProxyEnabled = table.Column<bool>(type: "bit", nullable: false),
                    ProxyApproverId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_OvertimeApprover", x => x.OvertimeApproverId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimePolicy",
                columns: table => new
                {
                    OvertimeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OvertimeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OvertimeNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AmountType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsFlatAmountType = table.Column<bool>(type: "bit", nullable: false),
                    IsPercentageAmountType = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LimitationOfUnit = table.Column<bool>(type: "bit", nullable: false),
                    MaxUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinUnit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LimitationOfAmount = table.Column<bool>(type: "bit", nullable: false),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_OvertimePolicy", x => x.OvertimeId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeProcess",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryMonth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_OvertimeProcess", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeRequest",
                columns: table => new
                {
                    OvertimeRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorityId = table.Column<long>(type: "bigint", nullable: false),
                    OvertimeId = table.Column<long>(type: "bigint", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WaitingStage = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_OvertimeRequest", x => x.OvertimeRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeTeamApprovalMapping",
                columns: table => new
                {
                    OvertimeTeamApprovalMappingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OvertimeApproverId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ApprovalLevel = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_OvertimeTeamApprovalMapping", x => x.OvertimeTeamApprovalMappingId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_UploadOvertimeAllowances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    OvertimeId = table.Column<long>(type: "bigint", nullable: false),
                    OvertimeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsUnitUpload = table.Column<bool>(type: "bit", nullable: false),
                    Unit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalaryMonth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAmountUpload = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_UploadOvertimeAllowances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeAllowances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OvertimeProcessId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    OvertimeId = table.Column<long>(type: "bigint", nullable: false),
                    OvertimeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryMonth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_OvertimeAllowances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_OvertimeAllowances_Payroll_OvertimeProcess_OvertimeProcessId",
                        column: x => x.OvertimeProcessId,
                        principalTable: "Payroll_OvertimeProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeProcessDetails",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OvertimeProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_OvertimeProcessDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_OvertimeProcessDetails_Payroll_OvertimeProcess_OvertimeProcessId",
                        column: x => x.OvertimeProcessId,
                        principalTable: "Payroll_OvertimeProcess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_OvertimeRequestDetails",
                columns: table => new
                {
                    OvertimeRequestDetailsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OvertimeApproverId = table.Column<long>(type: "bigint", nullable: false),
                    ApprovalOrder = table.Column<int>(type: "int", nullable: false),
                    ActionRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsReverted = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OvertimeRequestId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_OvertimeRequestDetails", x => x.OvertimeRequestDetailsId);
                    table.ForeignKey(
                        name: "FK_Payroll_OvertimeRequestDetails_Payroll_OvertimeRequest_OvertimeRequestId",
                        column: x => x.OvertimeRequestId,
                        principalTable: "Payroll_OvertimeRequest",
                        principalColumn: "OvertimeRequestId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_OvertimeAllowances_OvertimeProcessId",
                table: "Payroll_OvertimeAllowances",
                column: "OvertimeProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_OvertimeProcessDetails_OvertimeProcessId",
                table: "Payroll_OvertimeProcessDetails",
                column: "OvertimeProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_OvertimeRequestDetails_OvertimeRequestId",
                table: "Payroll_OvertimeRequestDetails",
                column: "OvertimeRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payroll_OvertimeAllowances");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimeApprovalLevel");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimeApprover");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimePolicy");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimeProcessDetails");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimeRequestDetails");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimeTeamApprovalMapping");

            migrationBuilder.DropTable(
                name: "Payroll_UploadOvertimeAllowances");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimeProcess");

            migrationBuilder.DropTable(
                name: "Payroll_OvertimeRequest");
        }
    }
}
