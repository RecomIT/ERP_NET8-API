using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Attendance.Migrations
{
    /// <inheritdoc />
    public partial class AttendanceModuleInit25May2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_AttendanceProcess",
                columns: table => new
                {
                    AttendanceProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    MonthYear = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    LockedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LockedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_HR_AttendanceProcess", x => x.AttendanceProcessId);
                });

            migrationBuilder.CreateTable(
                name: "HR_AttendanceRowData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MachineId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_AttendanceRowData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_AttendanceStatus",
                columns: table => new
                {
                    StatusCode = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    StatusDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_AttendanceStatus", x => x.StatusCode);
                });

            migrationBuilder.CreateTable(
                name: "HR_EarlyDeparture",
                columns: table => new
                {
                    EarlyDepartureId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    AppliedDate = table.Column<DateTime>(type: "date", nullable: true),
                    AppliedTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EmpEmailNotificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminEmailNotificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorId = table.Column<long>(type: "bigint", nullable: true),
                    LateReasonId = table.Column<long>(type: "bigint", nullable: true),
                    OtherReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    RequestedForDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AcceptedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptorRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EarlyDeparture", x => x.EarlyDepartureId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeComplianceDailyAttendance",
                columns: table => new
                {
                    AttendanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: true),
                    DivisionId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesiginationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCardNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FingerId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    InTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OutTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    InHour = table.Column<short>(type: "smallint", nullable: true),
                    InMinute = table.Column<short>(type: "smallint", nullable: true),
                    OutHour = table.Column<short>(type: "smallint", nullable: true),
                    OutMinute = table.Column<short>(type: "smallint", nullable: true),
                    LateInMinutes = table.Column<short>(type: "smallint", nullable: true),
                    OTHours = table.Column<short>(type: "smallint", nullable: true),
                    OTMinutes = table.Column<short>(type: "smallint", nullable: true),
                    IsFromMachine = table.Column<bool>(type: "bit", nullable: false),
                    TotalWorkingHours = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InTimeMachineNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OutTimeMachineNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeLeaveReqId = table.Column<long>(type: "bigint", nullable: true),
                    LeaveTypeId = table.Column<long>(type: "bigint", nullable: true),
                    ManualReqId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SchedulerInfoId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsWorkingDay = table.Column<bool>(type: "bit", nullable: true),
                    IsWeekend = table.Column<bool>(type: "bit", nullable: true),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: true),
                    IsLeaveDay = table.Column<bool>(type: "bit", nullable: true),
                    IsPresent = table.Column<bool>(type: "bit", nullable: true),
                    IsLate = table.Column<bool>(type: "bit", nullable: true),
                    LeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeComplianceDailyAttendance", x => x.AttendanceId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeComplianceMonthlyAttendenceSummery",
                columns: table => new
                {
                    SummeryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: true),
                    DivisionId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeCardNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    PresentQty = table.Column<decimal>(type: "decimal(18,2)", maxLength: 20, nullable: true),
                    AbsentQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WeekendQty = table.Column<short>(type: "smallint", nullable: true),
                    WorkQtyAtWeekend = table.Column<short>(type: "smallint", nullable: true),
                    HolidayQty = table.Column<short>(type: "smallint", nullable: true),
                    WorkQtyAtHoliday = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FullDayLeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HalfDayLeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WorkQtyAtLeave = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LateQty = table.Column<short>(type: "smallint", nullable: true),
                    TotalWokingDay = table.Column<short>(type: "smallint", nullable: true),
                    TotalOT = table.Column<TimeSpan>(type: "time", nullable: true),
                    AboutLeaveBalance = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeComplianceMonthlyAttendenceSummery", x => x.SummeryId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeDailyAttendance",
                columns: table => new
                {
                    AttendanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: true),
                    DivisionId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesiginationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCardNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FingerId = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    InTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OutTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    InHour = table.Column<short>(type: "smallint", nullable: true),
                    InMinute = table.Column<short>(type: "smallint", nullable: true),
                    OutHour = table.Column<short>(type: "smallint", nullable: true),
                    OutMinute = table.Column<short>(type: "smallint", nullable: true),
                    TotalLateTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OT = table.Column<TimeSpan>(type: "time", nullable: true),
                    OTHours = table.Column<short>(type: "smallint", nullable: true),
                    OTMinutes = table.Column<short>(type: "smallint", nullable: true),
                    IsFromMachine = table.Column<bool>(type: "bit", nullable: false),
                    TotalWorkingHours = table.Column<TimeSpan>(type: "time", nullable: true),
                    InTimeMachineNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OutTimeMachineNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearlyHolidayId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeLeaveReqId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LeaveTypeId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManualReqId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SchedulerInfoId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsWorkingDay = table.Column<bool>(type: "bit", nullable: true),
                    IsWeekend = table.Column<bool>(type: "bit", nullable: true),
                    IsHoliday = table.Column<bool>(type: "bit", nullable: true),
                    IsLeaveDay = table.Column<bool>(type: "bit", nullable: true),
                    IsPresent = table.Column<bool>(type: "bit", nullable: true),
                    IsLate = table.Column<bool>(type: "bit", nullable: true),
                    HasEmailSent = table.Column<bool>(type: "bit", nullable: true),
                    LateSetId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeDailyAttendance", x => x.AttendanceId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeManualAttendance",
                columns: table => new
                {
                    ManualAttendanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManualAttendanceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    AttendanceDate = table.Column<DateTime>(type: "date", nullable: true),
                    TimeRequestFor = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    InTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OutTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AcceptedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptorRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmployeeManualAttendance", x => x.ManualAttendanceId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeMonthlyAttendenceSummery",
                columns: table => new
                {
                    SummeryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: true),
                    DivisionId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeCardNo = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    FingerId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    PresentQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AbsentQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WeekendQty = table.Column<short>(type: "smallint", nullable: true),
                    WorkQtyAtWeekend = table.Column<short>(type: "smallint", nullable: true),
                    HolidayQty = table.Column<short>(type: "smallint", nullable: true),
                    WorkQtyAtHoliday = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FullDayLeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HalfDayLeaveQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WorkQtyAtLeave = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LateQty = table.Column<short>(type: "smallint", nullable: true),
                    TotalWokingDay = table.Column<short>(type: "smallint", nullable: true),
                    TotalOT = table.Column<TimeSpan>(type: "time", nullable: true),
                    AboutLeaveBalance = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeMonthlyAttendenceSummery", x => x.SummeryId);
                });

            migrationBuilder.CreateTable(
                name: "HR_GeoLocationAttendance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AttendanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AttendanceInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceInLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceOutLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceInRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceOutRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_GeoLocationAttendance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_LateReasons",
                columns: table => new
                {
                    LateReasonId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LateReasonName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AcceptedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptorRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LateReasons", x => x.LateReasonId);
                });

            migrationBuilder.CreateTable(
                name: "HR_LateRequests",
                columns: table => new
                {
                    LateRequestsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmloyeeId = table.Column<long>(type: "bigint", nullable: true),
                    AppliedDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmailNotificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupervisorId = table.Column<long>(type: "bigint", nullable: true),
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
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AcceptedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AcceptedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcceptorRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LateRequests", x => x.LateRequestsId);
                });

            migrationBuilder.CreateTable(
                name: "HR_PublicHolidays",
                columns: table => new
                {
                    PublicHolidayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TitleInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Date = table.Column<short>(type: "smallint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsDepandentOnMoon = table.Column<bool>(type: "bit", nullable: false),
                    ReligionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MonthName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    table.PrimaryKey("PK_HR_PublicHolidays", x => x.PublicHolidayId);
                });

            migrationBuilder.CreateTable(
                name: "HR_SchedulerInfo",
                columns: table => new
                {
                    SchedulerInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScheduleCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HostEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Details = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ScheduleDate = table.Column<DateTime>(type: "date", nullable: true),
                    FromTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ToTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SchedulerInfo", x => x.SchedulerInfoId);
                });

            migrationBuilder.CreateTable(
                name: "HR_WorkShifts",
                columns: table => new
                {
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NameDetail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    NameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NameDetailInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    InBufferTime = table.Column<short>(type: "smallint", nullable: false),
                    MaxInTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    LunchStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    LunchEndTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    OTStartTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    MaxOTHour = table.Column<short>(type: "smallint", nullable: false),
                    MaxBeforeTime = table.Column<short>(type: "smallint", nullable: false),
                    MaxAfterTime = table.Column<short>(type: "smallint", nullable: false),
                    ExceededMaxAfterTime = table.Column<short>(type: "smallint", nullable: false),
                    WeekendDayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_HR_WorkShifts", x => x.WorkShiftId);
                });

            migrationBuilder.CreateTable(
                name: "HR_YearlyHolidays",
                columns: table => new
                {
                    YearlyHolidayId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TitleInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StartMonth = table.Column<short>(type: "smallint", nullable: false),
                    StartYear = table.Column<short>(type: "smallint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndMonth = table.Column<short>(type: "smallint", nullable: false),
                    EndYear = table.Column<short>(type: "smallint", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    SpecifiedFor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DesignationId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SectionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DivisionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicHolidayId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_HR_YearlyHolidays", x => x.YearlyHolidayId);
                });

            migrationBuilder.CreateTable(
                name: "HR_LateRequestsDetail",
                columns: table => new
                {
                    LateRequestsDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestedForDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LateReasonId = table.Column<long>(type: "bigint", nullable: true),
                    OtherReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    LateRequestsId = table.Column<long>(type: "bigint", nullable: false),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LateRequestsDetail", x => x.LateRequestsDetailId);
                    table.ForeignKey(
                        name: "FK_HR_LateRequestsDetail_HR_LateRequests_LateRequestsId",
                        column: x => x.LateRequestsId,
                        principalTable: "HR_LateRequests",
                        principalColumn: "LateRequestsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_SchedulerDetail",
                columns: table => new
                {
                    SchedulerDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ParticipantStatus = table.Column<bool>(type: "bit", nullable: true),
                    ParticipantRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ScheduleDate = table.Column<DateTime>(type: "date", nullable: true),
                    FromTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    ToTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    SchedulerInfoId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_HR_SchedulerDetail", x => x.SchedulerDetailId);
                    table.ForeignKey(
                        name: "FK_HR_SchedulerDetail_HR_SchedulerInfo_SchedulerInfoId",
                        column: x => x.SchedulerInfoId,
                        principalTable: "HR_SchedulerInfo",
                        principalColumn: "SchedulerInfoId");
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeWorkShifts",
                columns: table => new
                {
                    EmployeeWorkShiftId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    InActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubsectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentWorkShiftId = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_HR_EmployeeWorkShifts", x => x.EmployeeWorkShiftId);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeWorkShifts_HR_WorkShifts_WorkShiftId",
                        column: x => x.WorkShiftId,
                        principalTable: "HR_WorkShifts",
                        principalColumn: "WorkShiftId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_WorkShiftWeekends",
                columns: table => new
                {
                    ShiftWeekendId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WorkShiftName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_WorkShiftWeekends", x => x.ShiftWeekendId);
                    table.ForeignKey(
                        name: "FK_HR_WorkShiftWeekends_HR_WorkShifts_WorkShiftId",
                        column: x => x.WorkShiftId,
                        principalTable: "HR_WorkShifts",
                        principalColumn: "WorkShiftId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_AttendanceProcess_NonClusteredIndex",
                table: "HR_AttendanceProcess",
                columns: new[] { "Month", "Year", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_AttendanceRowData_NonClusteredIndex",
                table: "HR_AttendanceRowData",
                columns: new[] { "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeComplianceDailyAttendance_NonClusteredIndex",
                table: "HR_EmployeeComplianceDailyAttendance",
                columns: new[] { "EmployeeId", "WorkShiftId", "TransactionDate", "Status", "LeaveTypeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeComplianceMonthlyAttendenceSummery_NonClusteredIndex",
                table: "HR_EmployeeComplianceMonthlyAttendenceSummery",
                columns: new[] { "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeDailyAttendance_NonClusteredIndex",
                table: "HR_EmployeeDailyAttendance",
                columns: new[] { "EmployeeId", "WorkShiftId", "TransactionDate", "Status", "LeaveTypeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeManualAttendance_NonClusteredIndex",
                table: "HR_EmployeeManualAttendance",
                columns: new[] { "ManualAttendanceCode", "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeMonthlyAttendenceSummery_NonClusteredIndex",
                table: "HR_EmployeeMonthlyAttendenceSummery",
                columns: new[] { "EmployeeId", "Year", "Month", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeWorkShifts_NonClusteredIndex",
                table: "HR_EmployeeWorkShifts",
                columns: new[] { "EmployeeId", "StateStatus", "Flag", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeWorkShifts_WorkShiftId",
                table: "HR_EmployeeWorkShifts",
                column: "WorkShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_LateRequestsDetail_LateRequestsId",
                table: "HR_LateRequestsDetail",
                column: "LateRequestsId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_PublicHolidays_NonClusteredIndex",
                table: "HR_PublicHolidays",
                columns: new[] { "Title", "ReligionName", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_SchedulerDetail_NonClusteredIndex",
                table: "HR_SchedulerDetail",
                columns: new[] { "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_SchedulerDetail_SchedulerInfoId",
                table: "HR_SchedulerDetail",
                column: "SchedulerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_SchedulerInfo_NonClusteredIndex",
                table: "HR_SchedulerInfo",
                columns: new[] { "ScheduleCode", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_WorkShifts_NonClusteredIndex",
                table: "HR_WorkShifts",
                columns: new[] { "Title", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_WorkShiftWeekends_NonClusteredIndex",
                table: "HR_WorkShiftWeekends",
                columns: new[] { "DayName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_WorkShiftWeekends_WorkShiftId",
                table: "HR_WorkShiftWeekends",
                column: "WorkShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_YearlyHolidays_NonClusteredIndex",
                table: "HR_YearlyHolidays",
                columns: new[] { "Title", "StartDate", "EndDate", "Type", "StateStatus", "CompanyId", "OrganizationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_AttendanceProcess");

            migrationBuilder.DropTable(
                name: "HR_AttendanceRowData");

            migrationBuilder.DropTable(
                name: "HR_AttendanceStatus");

            migrationBuilder.DropTable(
                name: "HR_EarlyDeparture");

            migrationBuilder.DropTable(
                name: "HR_EmployeeComplianceDailyAttendance");

            migrationBuilder.DropTable(
                name: "HR_EmployeeComplianceMonthlyAttendenceSummery");

            migrationBuilder.DropTable(
                name: "HR_EmployeeDailyAttendance");

            migrationBuilder.DropTable(
                name: "HR_EmployeeManualAttendance");

            migrationBuilder.DropTable(
                name: "HR_EmployeeMonthlyAttendenceSummery");

            migrationBuilder.DropTable(
                name: "HR_EmployeeWorkShifts");

            migrationBuilder.DropTable(
                name: "HR_GeoLocationAttendance");

            migrationBuilder.DropTable(
                name: "HR_LateReasons");

            migrationBuilder.DropTable(
                name: "HR_LateRequestsDetail");

            migrationBuilder.DropTable(
                name: "HR_PublicHolidays");

            migrationBuilder.DropTable(
                name: "HR_SchedulerDetail");

            migrationBuilder.DropTable(
                name: "HR_WorkShiftWeekends");

            migrationBuilder.DropTable(
                name: "HR_YearlyHolidays");

            migrationBuilder.DropTable(
                name: "HR_LateRequests");

            migrationBuilder.DropTable(
                name: "HR_SchedulerInfo");

            migrationBuilder.DropTable(
                name: "HR_WorkShifts");
        }
    }
}
