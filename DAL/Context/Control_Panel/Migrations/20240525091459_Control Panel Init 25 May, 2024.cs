using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Control_Panel.Migrations
{
    /// <inheritdoc />
    public partial class ControlPanelInit25May2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PublicIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrivateIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OSVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BrowserVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MACID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LoggedInTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoggedOutTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTokens", x => new { x.UserId, x.LoginProvider, x.Name, x.SessionId });
                });

            migrationBuilder.CreateTable(
                name: "tblActivityLogger",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActionMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    ActionDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpactTables = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogInIP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MACID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblActivityLogger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblApplications",
                columns: table => new
                {
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ApplicationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblApplications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "tblBranches",
                columns: table => new
                {
                    BranchId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchUniqueId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    BranchCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DivisionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBranches", x => x.BranchId);
                });

            migrationBuilder.CreateTable(
                name: "tblCompanyAuthorization",
                columns: table => new
                {
                    ComAuthId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    MainMenuName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCompanyAuthorization", x => x.ComAuthId);
                });

            migrationBuilder.CreateTable(
                name: "tblConfigurable",
                columns: table => new
                {
                    ConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ConfigText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblConfigurable", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "tblEmailSetting",
                columns: table => new
                {
                    EmailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EmailPassword = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailFor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsBodyHtml = table.Column<bool>(type: "bit", nullable: false),
                    EnableSsl = table.Column<bool>(type: "bit", nullable: false),
                    UseDefaultCredentials = table.Column<bool>(type: "bit", nullable: false),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Host = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EmailHtmlBody = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailTextBody = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmailSetting", x => x.EmailId);
                });

            migrationBuilder.CreateTable(
                name: "tblExceptionLogger",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HelpLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MethodName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LineNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ErrorNumber = table.Column<int>(type: "int", nullable: true),
                    ErrorState = table.Column<int>(type: "int", nullable: true),
                    ErrorSeverity = table.Column<int>(type: "int", nullable: true),
                    ErrorDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    LogInIP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PCName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MACID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblExceptionLogger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblHRModuleConfig",
                columns: table => new
                {
                    HRModuleConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    AttendanceProcess = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EnableMaxLateWarning = table.Column<bool>(type: "bit", nullable: true),
                    MaxLateInMonth = table.Column<short>(type: "smallint", nullable: true),
                    EnableSequenceLateWarning = table.Column<bool>(type: "bit", nullable: true),
                    SequenceLateInMonth = table.Column<short>(type: "smallint", nullable: true),
                    LeaveStartMonth = table.Column<short>(type: "smallint", nullable: true),
                    LeaveEndMonth = table.Column<short>(type: "smallint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblHRModuleConfig", x => x.HRModuleConfigId);
                });

            migrationBuilder.CreateTable(
                name: "tblModuleConfig",
                columns: table => new
                {
                    ModuleConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigId = table.Column<long>(type: "bigint", nullable: false),
                    ConfigCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ConfigText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ConfigValue = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblModuleConfig", x => x.ModuleConfigId);
                });

            migrationBuilder.CreateTable(
                name: "tblOrganizationAuthorization",
                columns: table => new
                {
                    OrgAuthId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrganizationAuthorization", x => x.OrgAuthId);
                });

            migrationBuilder.CreateTable(
                name: "tblOrganizations",
                columns: table => new
                {
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrgUniqueId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrgCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SiteThumbnailPath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ContractStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    ContractExpireDate = table.Column<DateTime>(type: "date", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OrgPic = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    OrgImageFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrgLogoPath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReportPic = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ReportImageFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReportLogoPath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AppId = table.Column<long>(type: "bigint", nullable: true),
                    AppName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    StorageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrganizations", x => x.OrganizationId);
                });

            migrationBuilder.CreateTable(
                name: "tblOTPRequests",
                columns: table => new
                {
                    RequestId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PublicIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrivateIP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OS = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OSVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BrowserVersion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    OTP = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true),
                    OTPLifeTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    VerifiedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOTPRequests", x => x.RequestId);
                });

            migrationBuilder.CreateTable(
                name: "tblPayrollModuleConfig",
                columns: table => new
                {
                    PayrollModuleConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    WhatDoesConsiderationForMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProvidentFundactivated = table.Column<bool>(type: "bit", nullable: true),
                    BaseOfProvidentFund = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PercentageOfProvidentFund = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalculateTaxOnArrearAmount = table.Column<bool>(type: "bit", nullable: false),
                    PercentageOfActualCalculatedTaxForMonthlyDeduction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsOnceOffTaxAvailable = table.Column<bool>(type: "bit", nullable: true),
                    WhenDoesOnceOffTaxCutDown = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNonResidentTaxApplied = table.Column<bool>(type: "bit", nullable: true),
                    IsFestivalBonusDisbursedbasedonReligion = table.Column<bool>(type: "bit", nullable: true),
                    DiscontinuedEmployeesLastMonthPaymentProcess = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoticePayBasedOn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitOfBonus = table.Column<decimal>(type: "decimal(18,2)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPayrollModuleConfig", x => x.PayrollModuleConfigId);
                });

            migrationBuilder.CreateTable(
                name: "tblPFModuleConfig",
                columns: table => new
                {
                    PFModuleConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CalculateByJoiningDate = table.Column<bool>(type: "bit", nullable: true),
                    CashFlow = table.Column<bool>(type: "bit", nullable: true),
                    Subsidiary = table.Column<bool>(type: "bit", nullable: true),
                    OnlyEmployeePartLoan = table.Column<bool>(type: "bit", nullable: true),
                    IsIslamic = table.Column<bool>(type: "bit", nullable: true),
                    MonthWiseIntrument = table.Column<bool>(type: "bit", nullable: true),
                    PendingContribution = table.Column<bool>(type: "bit", nullable: true),
                    GenerateAmortization = table.Column<bool>(type: "bit", nullable: true),
                    LoanPaidandAmortization = table.Column<bool>(type: "bit", nullable: true),
                    ReceivePaymentReport = table.Column<bool>(type: "bit", nullable: true),
                    ContributionFromPayroll = table.Column<bool>(type: "bit", nullable: true),
                    InstrumentAccruedProcess = table.Column<bool>(type: "bit", nullable: true),
                    Forfeiture = table.Column<bool>(type: "bit", nullable: true),
                    Monthlyprofit = table.Column<bool>(type: "bit", nullable: true),
                    Chequeue = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPFModuleConfig", x => x.PFModuleConfigId);
                });

            migrationBuilder.CreateTable(
                name: "tblReportAuthorization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReporCategorytId = table.Column<long>(type: "bigint", nullable: false),
                    ReporSubCategorytId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    ModuleId = table.Column<long>(type: "bigint", nullable: true),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: true),
                    SubmenuId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReportAuthorization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblReportCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReportCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblReportConfig",
                columns: table => new
                {
                    ReportConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportCategory = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReportPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubReport1ReportPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubReport1Process = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubReport2ReportPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SubReport2Process = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MinifiedReportPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FinalReportPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Month = table.Column<short>(type: "smallint", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProcessName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    FiscalYearRange = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeactiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    AuthorizorId = table.Column<long>(type: "bigint", nullable: true),
                    SignaturePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ReportCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ReportSubCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ReportAuthorizationId = table.Column<long>(type: "bigint", nullable: true),
                    ReportSignatoriesId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReportConfig", x => x.ReportConfigId);
                });

            migrationBuilder.CreateTable(
                name: "tblRoleAuthorization",
                columns: table => new
                {
                    TaskId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    SubmenuId = table.Column<long>(type: "bigint", nullable: false),
                    ParentSubmenuId = table.Column<long>(type: "bigint", nullable: false),
                    IsSubmenuPermission = table.Column<bool>(type: "bit", nullable: false),
                    IsPageTabPermission = table.Column<bool>(type: "bit", nullable: false),
                    HasTab = table.Column<bool>(type: "bit", nullable: false),
                    Add = table.Column<bool>(type: "bit", nullable: false),
                    Edit = table.Column<bool>(type: "bit", nullable: false),
                    Detail = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    Check = table.Column<bool>(type: "bit", nullable: false),
                    Approval = table.Column<bool>(type: "bit", nullable: false),
                    Accept = table.Column<bool>(type: "bit", nullable: false),
                    Report = table.Column<bool>(type: "bit", nullable: false),
                    Upload = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoleAuthorization", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "tblUserAuthorization",
                columns: table => new
                {
                    TaskId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    MainmenuId = table.Column<long>(type: "bigint", nullable: false),
                    SubmenuId = table.Column<long>(type: "bigint", nullable: false),
                    ParentSubmenuId = table.Column<long>(type: "bigint", nullable: false),
                    IsSubmenuPermission = table.Column<bool>(type: "bit", nullable: false),
                    IsPageTabPermission = table.Column<bool>(type: "bit", nullable: false),
                    HasTab = table.Column<bool>(type: "bit", nullable: false),
                    Add = table.Column<bool>(type: "bit", nullable: false),
                    Edit = table.Column<bool>(type: "bit", nullable: false),
                    Detail = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    Approval = table.Column<bool>(type: "bit", nullable: false),
                    Check = table.Column<bool>(type: "bit", nullable: false),
                    Accept = table.Column<bool>(type: "bit", nullable: false),
                    Report = table.Column<bool>(type: "bit", nullable: false),
                    Upload = table.Column<bool>(type: "bit", nullable: false),
                    DivisionId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUserAuthorization", x => x.TaskId);
                });

            migrationBuilder.CreateTable(
                name: "tblModules",
                columns: table => new
                {
                    ModuleId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblModules", x => x.ModuleId);
                    table.ForeignKey(
                        name: "FK_tblModules_tblApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "tblApplications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRoleActive = table.Column<bool>(type: "bit", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DivisionId = table.Column<long>(type: "bigint", nullable: false),
                    IsDefaultPassword = table.Column<bool>(type: "bit", nullable: true),
                    PasswordChangedCount = table.Column<int>(type: "int", nullable: false),
                    DefaultCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DefaultPasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultSecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordExpiredDate = table.Column<DateTime>(type: "date", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_tblBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "tblBranches",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsSysadmin = table.Column<bool>(type: "bit", nullable: true),
                    IsGroupAdmin = table.Column<bool>(type: "bit", nullable: true),
                    IsCompanyAdmin = table.Column<bool>(type: "bit", nullable: true),
                    IsBranchAdmin = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_tblOrganizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "tblOrganizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblCompanies",
                columns: table => new
                {
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComUniqueId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SiteThumbnailPath = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ContractStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    ContractExpireDate = table.Column<DateTime>(type: "date", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CompanyPic = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CompanyLogoPath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    CompanyImageFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReportPic = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ReportLogoPath = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReportImageFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceDeviceLocation = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCompanies", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_tblCompanies_tblOrganizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "tblOrganizations",
                        principalColumn: "OrganizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblReportSubCategory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReportCategoryId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblReportSubCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblReportSubCategory_tblReportCategory_ReportCategoryId",
                        column: x => x.ReportCategoryId,
                        principalTable: "tblReportCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblRoleAuthTab",
                columns: table => new
                {
                    RATId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RoleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubmenuId = table.Column<long>(type: "bigint", nullable: false),
                    TabId = table.Column<long>(type: "bigint", nullable: false),
                    TabName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Add = table.Column<bool>(type: "bit", nullable: false),
                    Edit = table.Column<bool>(type: "bit", nullable: false),
                    Detail = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    Approval = table.Column<bool>(type: "bit", nullable: false),
                    Report = table.Column<bool>(type: "bit", nullable: false),
                    Check = table.Column<bool>(type: "bit", nullable: false),
                    Accept = table.Column<bool>(type: "bit", nullable: false),
                    Upload = table.Column<bool>(type: "bit", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    TaskId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblRoleAuthTab", x => x.RATId);
                    table.ForeignKey(
                        name: "FK_tblRoleAuthTab_tblRoleAuthorization_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tblRoleAuthorization",
                        principalColumn: "TaskId");
                });

            migrationBuilder.CreateTable(
                name: "tblUserAuthTab",
                columns: table => new
                {
                    UATId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<long>(type: "bigint", nullable: true),
                    SubmenuId = table.Column<long>(type: "bigint", nullable: false),
                    TabId = table.Column<long>(type: "bigint", nullable: false),
                    TabName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Add = table.Column<bool>(type: "bit", nullable: false),
                    Edit = table.Column<bool>(type: "bit", nullable: false),
                    Detail = table.Column<bool>(type: "bit", nullable: false),
                    Delete = table.Column<bool>(type: "bit", nullable: false),
                    Approval = table.Column<bool>(type: "bit", nullable: false),
                    Check = table.Column<bool>(type: "bit", nullable: false),
                    Accept = table.Column<bool>(type: "bit", nullable: false),
                    Report = table.Column<bool>(type: "bit", nullable: false),
                    Upload = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_tblUserAuthTab", x => x.UATId);
                    table.ForeignKey(
                        name: "FK_tblUserAuthTab_tblUserAuthorization_TaskId",
                        column: x => x.TaskId,
                        principalTable: "tblUserAuthorization",
                        principalColumn: "TaskId");
                });

            migrationBuilder.CreateTable(
                name: "tblMainMenus",
                columns: table => new
                {
                    MMId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IconColor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MId = table.Column<long>(type: "bigint", nullable: false),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SequenceNo = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblMainMenus", x => x.MMId);
                    table.ForeignKey(
                        name: "FK_tblMainMenus_tblModules_MId",
                        column: x => x.MId,
                        principalTable: "tblModules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblDivisions",
                columns: table => new
                {
                    DivisionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DIVCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDivisions", x => x.DivisionId);
                    table.ForeignKey(
                        name: "FK_tblDivisions_tblCompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "tblCompanies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblSubMenus",
                columns: table => new
                {
                    SubmenuId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubmenuName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ControllerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Component = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IconColor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsViewable = table.Column<bool>(type: "bit", nullable: false),
                    IsActAsParent = table.Column<bool>(type: "bit", nullable: false),
                    HasTab = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    MenuSequence = table.Column<int>(type: "int", nullable: true),
                    ParentSubMenuId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MMId = table.Column<long>(type: "bigint", nullable: false),
                    ModuleId = table.Column<long>(type: "bigint", nullable: false),
                    ApplicationId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSubMenus", x => x.SubmenuId);
                    table.ForeignKey(
                        name: "FK_tblSubMenus_tblMainMenus_MMId",
                        column: x => x.MMId,
                        principalTable: "tblMainMenus",
                        principalColumn: "MMId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblPageTabs",
                columns: table => new
                {
                    TabId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TabName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IconClass = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IconColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SubmenuId = table.Column<long>(type: "bigint", nullable: true),
                    MMId = table.Column<long>(type: "bigint", nullable: false),
                    ComId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPageTabs", x => x.TabId);
                    table.ForeignKey(
                        name: "FK_tblPageTabs_tblSubMenus_SubmenuId",
                        column: x => x.SubmenuId,
                        principalTable: "tblSubMenus",
                        principalColumn: "SubmenuId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_OrganizationId",
                table: "AspNetRoles",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BranchId",
                table: "AspNetUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_tblCompanies_OrganizationId",
                table: "tblCompanies",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_tblDivisions_CompanyId",
                table: "tblDivisions",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_tblMainMenus_MId",
                table: "tblMainMenus",
                column: "MId");

            migrationBuilder.CreateIndex(
                name: "IX_tblModules_ApplicationId",
                table: "tblModules",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_tblPageTabs_SubmenuId",
                table: "tblPageTabs",
                column: "SubmenuId");

            migrationBuilder.CreateIndex(
                name: "IX_tblReportSubCategory_ReportCategoryId",
                table: "tblReportSubCategory",
                column: "ReportCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_tblRoleAuthTab_TaskId",
                table: "tblRoleAuthTab",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_tblSubMenus_MMId",
                table: "tblSubMenus",
                column: "MMId");

            migrationBuilder.CreateIndex(
                name: "IX_tblUserAuthTab_TaskId",
                table: "tblUserAuthTab",
                column: "TaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "tblActivityLogger");

            migrationBuilder.DropTable(
                name: "tblCompanyAuthorization");

            migrationBuilder.DropTable(
                name: "tblConfigurable");

            migrationBuilder.DropTable(
                name: "tblDivisions");

            migrationBuilder.DropTable(
                name: "tblEmailSetting");

            migrationBuilder.DropTable(
                name: "tblExceptionLogger");

            migrationBuilder.DropTable(
                name: "tblHRModuleConfig");

            migrationBuilder.DropTable(
                name: "tblModuleConfig");

            migrationBuilder.DropTable(
                name: "tblOrganizationAuthorization");

            migrationBuilder.DropTable(
                name: "tblOTPRequests");

            migrationBuilder.DropTable(
                name: "tblPageTabs");

            migrationBuilder.DropTable(
                name: "tblPayrollModuleConfig");

            migrationBuilder.DropTable(
                name: "tblPFModuleConfig");

            migrationBuilder.DropTable(
                name: "tblReportAuthorization");

            migrationBuilder.DropTable(
                name: "tblReportConfig");

            migrationBuilder.DropTable(
                name: "tblReportSubCategory");

            migrationBuilder.DropTable(
                name: "tblRoleAuthTab");

            migrationBuilder.DropTable(
                name: "tblUserAuthTab");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "tblCompanies");

            migrationBuilder.DropTable(
                name: "tblSubMenus");

            migrationBuilder.DropTable(
                name: "tblReportCategory");

            migrationBuilder.DropTable(
                name: "tblRoleAuthorization");

            migrationBuilder.DropTable(
                name: "tblUserAuthorization");

            migrationBuilder.DropTable(
                name: "tblBranches");

            migrationBuilder.DropTable(
                name: "tblOrganizations");

            migrationBuilder.DropTable(
                name: "tblMainMenus");

            migrationBuilder.DropTable(
                name: "tblModules");

            migrationBuilder.DropTable(
                name: "tblApplications");
        }
    }
}
