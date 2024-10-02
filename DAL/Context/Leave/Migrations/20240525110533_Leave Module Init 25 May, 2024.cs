using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Leave.Migrations
{
    /// <inheritdoc />
    public partial class LeaveModuleInit25May2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_EmployeeLeaveBalance",
                columns: table => new
                {
                    EmployeeLeaveBalanceId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    LeaveTypeId = table.Column<long>(type: "bigint", nullable: false),
                    LeaveYear = table.Column<short>(type: "smallint", nullable: false),
                    LeaveSettingId = table.Column<long>(type: "bigint", nullable: true),
                    LeavePeriodStart = table.Column<DateTime>(type: "date", nullable: true),
                    LeavePeriodEnd = table.Column<DateTime>(type: "date", nullable: true),
                    LeaveTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TotalLeave = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveApplied = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LeaveAvailed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    YearStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeLeaveBalance", x => new { x.EmployeeLeaveBalanceId, x.EmployeeId, x.LeaveTypeId, x.LeaveYear });
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeLeaveHistory",
                columns: table => new
                {
                    LeaveHistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Count = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: true),
                    LeaveTypeId = table.Column<long>(type: "bigint", nullable: true),
                    LeaveSettingId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LeaveDate = table.Column<DateTime>(type: "date", nullable: true),
                    ReplacementDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeLeaveRequestId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeLeaveBalanceId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmployeeLeaveHistory", x => x.LeaveHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeLeaveRequest",
                columns: table => new
                {
                    EmployeeLeaveRequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeLeaveCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    LeaveTypeId = table.Column<long>(type: "bigint", nullable: false),
                    LeaveTypeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DayLeaveType = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    HalfDayType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AppliedFromDate = table.Column<DateTime>(type: "date", nullable: true),
                    AppliedToDate = table.Column<DateTime>(type: "date", nullable: true),
                    AppliedTotalDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LeavePurpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EmergencyPhoneNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AddressDuringLeave = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ReliverId = table.Column<long>(type: "bigint", nullable: true),
                    ReliverDesignationId = table.Column<long>(type: "bigint", nullable: true),
                    AttachmentFileTypes = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AttachmentFileNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachmentFiles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedFromDate = table.Column<DateTime>(type: "date", nullable: true),
                    ApprovedToDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalApprovalDays = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeLeaveBalanceId = table.Column<long>(type: "bigint", nullable: true),
                    SupervisorId = table.Column<long>(type: "bigint", nullable: true),
                    HODId = table.Column<long>(type: "bigint", nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualFileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedDeliveryDate = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovalRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CancelledBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancelRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmployeeLeaveRequest", x => x.EmployeeLeaveRequestId);
                });

            migrationBuilder.CreateTable(
                name: "HR_LeaveTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TitleInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ShortNameInBangali = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SerialNo = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LeaveTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_LeaveSetting",
                columns: table => new
                {
                    LeaveSettingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveTypeId = table.Column<long>(type: "bigint", nullable: false),
                    MandatoryNumberOfDays = table.Column<bool>(type: "bit", nullable: false),
                    NoOfDays = table.Column<short>(type: "smallint", nullable: true),
                    IsProratedLeaveBalanceApplicable = table.Column<bool>(type: "bit", nullable: false),
                    MaxDaysLeaveAtATime = table.Column<short>(type: "smallint", nullable: true),
                    IsHolidayIncluded = table.Column<bool>(type: "bit", nullable: false),
                    IsDayOffIncluded = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    IsCarryForward = table.Column<bool>(type: "bit", nullable: false),
                    MaxDaysCarryForward = table.Column<short>(type: "smallint", nullable: true),
                    LeaveApplicableFor = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RequestDaysBeforeTakingLeave = table.Column<short>(type: "smallint", nullable: false),
                    FileAttachedOption = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsMinimumDaysRequiredForFileAttached = table.Column<bool>(type: "bit", nullable: true),
                    RequiredDaysForFileAttached = table.Column<short>(type: "smallint", nullable: true),
                    MaximumTimesInServicePeriod = table.Column<short>(type: "smallint", nullable: true),
                    IsMinimumServicePeroid = table.Column<bool>(type: "bit", nullable: true),
                    MinimumServicePeroid = table.Column<short>(type: "smallint", nullable: true),
                    IsConfirmationRequired = table.Column<bool>(type: "bit", nullable: false),
                    IsLeaveEncashable = table.Column<bool>(type: "bit", nullable: false),
                    MinEncashablePercentage = table.Column<short>(type: "smallint", nullable: true),
                    MaxEncashablePercentage = table.Column<short>(type: "smallint", nullable: true),
                    CalculateBalanceBasedOn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DaysPerCycle = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GainDaysPerCycle = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AcquiredViaOffDayWork = table.Column<bool>(type: "bit", nullable: true),
                    ShowFullCalender = table.Column<bool>(type: "bit", nullable: true),
                    DeadlineForUtilizationLeave = table.Column<short>(type: "smallint", nullable: true),
                    IsRequiredEstimatedDeliveryDate = table.Column<bool>(type: "bit", nullable: true),
                    IsRequiredToApplyMinimumDaysBeforeEDD = table.Column<bool>(type: "bit", nullable: true),
                    RequiredDaysBeforeEDD = table.Column<short>(type: "smallint", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NoOfDaysBN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: false),
                    MaxDaysLeaveAtATimeBN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DaysPastTodayOpenForLeave = table.Column<short>(type: "smallint", nullable: false),
                    DaysBeforeTodayOpenForLeave = table.Column<short>(type: "smallint", nullable: false),
                    UnitOfServicePeroid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_LeaveSetting", x => x.LeaveSettingId);
                    table.ForeignKey(
                        name: "FK_HR_LeaveSetting_HR_LeaveTypes_LeaveTypeId",
                        column: x => x.LeaveTypeId,
                        principalTable: "HR_LeaveTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeLeaveBalance_NonClusteredIndex",
                table: "HR_EmployeeLeaveBalance",
                columns: new[] { "EmployeeId", "LeaveTypeId", "LeaveSettingId", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeLeaveHistory_NonClusteredIndex",
                table: "HR_EmployeeLeaveHistory",
                columns: new[] { "LeaveHistoryId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeLeaveRequest_NonClusteredIndex",
                table: "HR_EmployeeLeaveRequest",
                columns: new[] { "EmployeeLeaveCode", "EmployeeId", "LeaveTypeId", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveSetting_LeaveTypeId",
                table: "HR_LeaveSetting",
                column: "LeaveTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveSetting_NonClusteredIndex",
                table: "HR_LeaveSetting",
                columns: new[] { "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_LeaveTypes_NonClusteredIndex",
                table: "HR_LeaveTypes",
                columns: new[] { "Title", "CompanyId", "OrganizationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_EmployeeLeaveBalance");

            migrationBuilder.DropTable(
                name: "HR_EmployeeLeaveHistory");

            migrationBuilder.DropTable(
                name: "HR_EmployeeLeaveRequest");

            migrationBuilder.DropTable(
                name: "HR_LeaveSetting");

            migrationBuilder.DropTable(
                name: "HR_LeaveTypes");
        }
    }
}
