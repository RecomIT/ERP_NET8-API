using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Employee.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeModuleInit25May2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_ActivityLogger",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EMPCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Controller = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ActionMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImpactTables = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PreviousValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PK = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogInIP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PCName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MACID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_ActivityLogger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_Banks",
                columns: table => new
                {
                    BankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Banks", x => x.BankId);
                });

            migrationBuilder.CreateTable(
                name: "HR_CompanyAccountInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankId = table.Column<long>(type: "bigint", nullable: false),
                    BankBranchId = table.Column<long>(type: "bigint", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_CompanyAccountInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_CompanyEvents",
                columns: table => new
                {
                    CompanyEventsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EventTitleInBengali = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_CompanyEvents", x => x.CompanyEventsId);
                });

            migrationBuilder.CreateTable(
                name: "HR_ContractualEmployment",
                columns: table => new
                {
                    ContractId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastContractEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ContractStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ServiceTenure = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ServiceDayLength = table.Column<int>(type: "int", nullable: true),
                    ServiceMonthLength = table.Column<int>(type: "int", nullable: true),
                    ServiceYearLength = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsTerminated = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    LastContractId = table.Column<long>(type: "bigint", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_ContractualEmployment", x => x.ContractId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Costcenter",
                columns: table => new
                {
                    CostCenterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostCenterName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CostCenterCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CostcenterNameInBengali = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Costcenter", x => x.CostCenterId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountryNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ISOCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NationalityInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "HR_DataLabel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Serial = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_HR_DataLabel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_Departments",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DepartmentNameInBengali = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FunctionalDivisionId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Departments", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "HR_DiscontinuedEmployee",
                columns: table => new
                {
                    DiscontinuedId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    LastWorkingDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculateFestivalBonusTaxProratedBasis = table.Column<bool>(type: "bit", nullable: true),
                    CalculateProjectionTaxProratedBasis = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Releasetype = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_DiscontinuedEmployee", x => x.DiscontinuedId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmailSendingConfiguration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailStage = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailCC1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailCC2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailCC3 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailBCC1 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailBCC2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmailTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmailSendingConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeAccountInfo",
                columns: table => new
                {
                    AccountInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    BankId = table.Column<int>(type: "int", nullable: true),
                    BankBranchId = table.Column<int>(type: "int", nullable: true),
                    AgentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    PaymentMode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    AccountNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActivationReason = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    DeactivationFrom = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeAccountInfo", x => x.AccountInfoId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeConfirmationProposal",
                columns: table => new
                {
                    ConfirmationProposalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ConfirmationDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalRatingScore = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppraiserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppraiserComment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    WithPFActivation = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeConfirmationProposal", x => x.ConfirmationProposalId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeCurrentSalaryBreakDown",
                columns: table => new
                {
                    SalaryBreakDownId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceHead = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PercentageOfTotalSalary = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeactivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeCurrentSalaryBreakDown", x => x.SalaryBreakDownId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeDetail",
                columns: table => new
                {
                    EmployeeDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PersonalMobileNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PersonalEmailAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AlternativeEmailAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Feet = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Inch = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NationalityId = table.Column<int>(type: "int", nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PresentAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PresentAddressCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PresentAddressZipCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PresentAddressContactNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PermanentAddressDistrict = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PermanentAddressUpazila = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PermanentAddressZipCode = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PermanentAddressContactNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RelationWithEmergencyContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyContactNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyContactAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactFax = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EmergencyContactEmailAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactPerson2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RelationWithEmergencyContactPerson2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyContactNo2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyContactAddress2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactFax2 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EmergencyContactEmailAddress2 = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsResidential = table.Column<bool>(type: "bit", nullable: true),
                    IsPhysicallyDisabled = table.Column<bool>(type: "bit", nullable: true),
                    IsFreedomFighter = table.Column<bool>(type: "bit", nullable: true),
                    IsMobility = table.Column<bool>(type: "bit", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhotoSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhotoFormat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActualPhotoName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Taxzone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MinimumTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NumberOfChild = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeDetail", x => x.EmployeeDetailId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeDocument",
                columns: table => new
                {
                    DocumentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActualFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    File = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeDocument", x => x.DocumentId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeHistory",
                columns: table => new
                {
                    EmployeeHistoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubsectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    TableFlag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeOfficeInfos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeePersonalInfos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeFamilyInfos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeNomineeInfos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeRelativesInfos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeExperienceInfos = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeSkillInfos = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeHistory", x => x.EmployeeHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeInformation",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Salutation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NickName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GradeId = table.Column<int>(type: "int", nullable: true),
                    DesignationId = table.Column<int>(type: "int", nullable: true),
                    InternalDesignationId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: true),
                    SubSectionId = table.Column<int>(type: "int", nullable: true),
                    UnitId = table.Column<int>(type: "int", nullable: true),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    DivisionId = table.Column<int>(type: "int", nullable: true),
                    AppointmentDate = table.Column<DateTime>(type: "date", nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "date", nullable: true),
                    DateOfConfirmation = table.Column<DateTime>(type: "date", nullable: true),
                    ContractStartDate = table.Column<DateTime>(type: "date", nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    JobCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    ProbationMonth = table.Column<short>(type: "smallint", nullable: true),
                    ProbationEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExtendedProbationMonth = table.Column<short>(type: "smallint", nullable: true),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: true),
                    OfficeMobile = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OfficeEmail = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FingerID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Taxzone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MinimumTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TINNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TINFilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    IsPFMember = table.Column<bool>(type: "bit", nullable: true),
                    PFActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsFBActive = table.Column<bool>(type: "bit", nullable: true),
                    FBActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    TerminationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculateProjectionTaxProratedBasis = table.Column<bool>(type: "bit", nullable: true),
                    CalculateFestivalBonusTaxProratedBasis = table.Column<bool>(type: "bit", nullable: true),
                    PreviousCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GlobalID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeInformation", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeePFActivation",
                columns: table => new
                {
                    PFActivationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    PFBasedAmount = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PFPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmployeeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PFEffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    PFActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    InActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ConfirmationProposalId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeePFActivation", x => x.PFActivationId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeProbationaryExtensionProposal",
                columns: table => new
                {
                    ProbationaryExtensionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ExtensionFrom = table.Column<DateTime>(type: "date", nullable: false),
                    ExtensionTo = table.Column<DateTime>(type: "date", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    TotalRatingScore = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppraiserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppraiserComment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeProbationaryExtensionProposal", x => x.ProbationaryExtensionId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeePromotionProposal",
                columns: table => new
                {
                    PromotionProposalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Head = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExistingValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExistingText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProposalValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProposalText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    InActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    InActiveBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    ActiveBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeePromotionProposal", x => x.PromotionProposalId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeTransferProposal",
                columns: table => new
                {
                    TransferProposalId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Head = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExistingValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExistingText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProposalValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProposalText = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    InActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    InActiveBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    ActiveBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeTransferProposal", x => x.TransferProposalId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeType",
                columns: table => new
                {
                    EmployeeTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EmployeeType", x => x.EmployeeTypeId);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmploymentConfirmationOrProbotion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ConfirmationDate = table.Column<DateTime>(type: "date", nullable: true),
                    ExtensionFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ExtensionTo = table.Column<DateTime>(type: "date", nullable: true),
                    TotalRatingScore = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppraiserId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AppraiserComment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmploymentStageInfoId = table.Column<long>(type: "bigint", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmploymentConfirmationOrProbotion", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_ExceptionLogger",
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
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    LogInIP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PCName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MACID = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_HR_ExceptionLogger", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_FunctionalDivision",
                columns: table => new
                {
                    FunctionalDivisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionalDivisionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FunctionalDivisionNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_FunctionalDivision", x => x.FunctionalDivisionId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Grades",
                columns: table => new
                {
                    GradeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GradeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    GradeNameInBengali = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Grades", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "HR_JobCategory",
                columns: table => new
                {
                    JobCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_JobCategory", x => x.JobCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Jobtypes",
                columns: table => new
                {
                    JobTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    JobTypeNameInBengali = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Jobtypes", x => x.JobTypeId);
                });

            migrationBuilder.CreateTable(
                name: "HR_LevelOfEducation",
                columns: table => new
                {
                    LevelOfEducationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NameInBengali = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_LevelOfEducation", x => x.LevelOfEducationId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Levels",
                columns: table => new
                {
                    LevelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LevelName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LevelNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Levels", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Line",
                columns: table => new
                {
                    LineId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LineName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LineNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LineCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_HR_Line", x => x.LineId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Locations",
                columns: table => new
                {
                    LocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocationNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PoliceStationId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Locations", x => x.LocationId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Sections",
                columns: table => new
                {
                    SectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SectionNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Sections", x => x.SectionId);
                });

            migrationBuilder.CreateTable(
                name: "HR_Units",
                columns: table => new
                {
                    UnitId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UnitNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SubSectionId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Units", x => x.UnitId);
                });

            migrationBuilder.CreateTable(
                name: "HR_BankBranches",
                columns: table => new
                {
                    BankBranchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankBranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    BankBranchNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BankId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_BankBranches", x => x.BankBranchId);
                    table.ForeignKey(
                        name: "FK_HR_BankBranches_HR_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "HR_Banks",
                        principalColumn: "BankId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_CompanyEventsCalendar",
                columns: table => new
                {
                    CompanyEventsCalendarId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventTitle = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EventTitleInBengali = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CompanyEventsId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_HR_CompanyEventsCalendar", x => x.CompanyEventsCalendarId);
                    table.ForeignKey(
                        name: "FK_HR_CompanyEventsCalendar_HR_CompanyEvents_CompanyEventsId",
                        column: x => x.CompanyEventsId,
                        principalTable: "HR_CompanyEvents",
                        principalColumn: "CompanyEventsId");
                });

            migrationBuilder.CreateTable(
                name: "HR_Divisions",
                columns: table => new
                {
                    DivisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DivisionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DivisionNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DivisionCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Divisions", x => x.DivisionId);
                    table.ForeignKey(
                        name: "FK_HR_Divisions_HR_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "HR_Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_DepartmentalDivision",
                columns: table => new
                {
                    DepartmentalDivisionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentalDivisionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartmentalDivisionNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_DepartmentalDivision", x => x.DepartmentalDivisionId);
                    table.ForeignKey(
                        name: "FK_HR_DepartmentalDivision_HR_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "HR_Departments",
                        principalColumn: "DepartmentId");
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeEducation",
                columns: table => new
                {
                    EmployeeEducationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LevelOfEducationId = table.Column<int>(type: "int", nullable: false),
                    DegreeId = table.Column<int>(type: "int", nullable: false),
                    Major = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InstitutionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ScaleDivisionClass = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    YearOfPassing = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_HR_EmployeeEducation", x => x.EmployeeEducationId);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeEducation_HR_EmployeeInformation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_EmployeeInformation",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeExperience",
                columns: table => new
                {
                    EmployeeExperienceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExCompanyname = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExCompanyBusinees = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExCompanyLocation = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ExCompanyDepartment = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExCompanyDesignation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ExCompanyExperience = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    EmploymentFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmploymentTo = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeExperience", x => x.EmployeeExperienceId);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeExperience_HR_EmployeeInformation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_EmployeeInformation",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeFamilyInfo",
                columns: table => new
                {
                    EmployeeFamilyInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Relatation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Age = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DateOfBirth = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LifeStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DocumentFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeFamilyInfo", x => x.EmployeeFamilyInfoId);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeFamilyInfo_HR_EmployeeInformation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_EmployeeInformation",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeHierarchy",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    SupervisorId = table.Column<long>(type: "bigint", nullable: true),
                    SupervisorName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LineManagerId = table.Column<long>(type: "bigint", nullable: true),
                    LineManagerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ManagerId = table.Column<long>(type: "bigint", nullable: true),
                    ManagerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HeadOfDepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    HeadOfDepartmentName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HRAuthorityId = table.Column<long>(type: "bigint", nullable: true),
                    HRAuthorityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ActivationDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeHierarchy", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeHierarchy_HR_EmployeeInformation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_EmployeeInformation",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeNomineeInfo",
                columns: table => new
                {
                    NomineeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_HR_EmployeeNomineeInfo", x => x.NomineeId);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeNomineeInfo_HR_EmployeeInformation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_EmployeeInformation",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeRelativesInfo",
                columns: table => new
                {
                    RelativeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Relation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
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
                    table.PrimaryKey("PK_HR_EmployeeRelativesInfo", x => x.RelativeId);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeRelativesInfo_HR_EmployeeInformation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_EmployeeInformation",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_EmployeeSkill",
                columns: table => new
                {
                    EmployeeSkillId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrainingName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Organization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TopicCovers = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FromDate = table.Column<DateTime>(type: "date", nullable: true),
                    ToDate = table.Column<DateTime>(type: "date", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SkillCertificateFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_HR_EmployeeSkill", x => x.EmployeeSkillId);
                    table.ForeignKey(
                        name: "FK_HR_EmployeeSkill_HR_EmployeeInformation_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "HR_EmployeeInformation",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_Designations",
                columns: table => new
                {
                    DesignationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DesignationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ShortName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DesignationNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DesignationGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalaryGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    GradeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Designations", x => x.DesignationId);
                    table.ForeignKey(
                        name: "FK_HR_Designations_HR_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "HR_Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_EducationalDegrees",
                columns: table => new
                {
                    EducatioalDegreeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DegreeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DegreeNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LevelOfEducationId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_EducationalDegrees", x => x.EducatioalDegreeId);
                    table.ForeignKey(
                        name: "FK_HR_EducationalDegrees_HR_LevelOfEducation_LevelOfEducationId",
                        column: x => x.LevelOfEducationId,
                        principalTable: "HR_LevelOfEducation",
                        principalColumn: "LevelOfEducationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_SubSections",
                columns: table => new
                {
                    SubSectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubSectionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubSectionNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_SubSections", x => x.SubSectionId);
                    table.ForeignKey(
                        name: "FK_HR_SubSections_HR_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "HR_Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_Districts",
                columns: table => new
                {
                    DistrictId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DistrictName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DistrictNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DistrictCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DivisionId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_Districts", x => x.DistrictId);
                    table.ForeignKey(
                        name: "FK_HR_Districts_HR_Divisions_DivisionId",
                        column: x => x.DivisionId,
                        principalTable: "HR_Divisions",
                        principalColumn: "DivisionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_PoliceStations",
                columns: table => new
                {
                    PoliceStationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PoliceStationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PoliceStationNameInBengali = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_PoliceStations", x => x.PoliceStationId);
                    table.ForeignKey(
                        name: "FK_HR_PoliceStations_HR_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "HR_Districts",
                        principalColumn: "DistrictId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_ActivityLogger_NonClusteredIndex",
                table: "HR_ActivityLogger",
                columns: new[] { "UserId", "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_BankBranches_BankId",
                table: "HR_BankBranches",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_BankBranches_NonClusteredIndex",
                table: "HR_BankBranches",
                columns: new[] { "BankBranchName", "RoutingNumber", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Banks_NonClusteredIndex",
                table: "HR_Banks",
                columns: new[] { "BankName", "BankCode", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_CompanyEventsCalendar_CompanyEventsId",
                table: "HR_CompanyEventsCalendar",
                column: "CompanyEventsId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_ContractualEmployment_NonClusteredIndex",
                table: "HR_ContractualEmployment",
                columns: new[] { "EmployeeId", "ContractCode", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Costcenter_NonClusteredIndex",
                table: "HR_Costcenter",
                columns: new[] { "CostCenterName", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Countries_NonClusteredIndex",
                table: "HR_Countries",
                columns: new[] { "CountryName", "ISOCode", "Nationality", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_DepartmentalDivision_DepartmentId",
                table: "HR_DepartmentalDivision",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_FunctionalDivision_NonClusteredIndex",
                table: "HR_DepartmentalDivision",
                columns: new[] { "DepartmentalDivisionName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Departments_NonClusteredIndex",
                table: "HR_Departments",
                columns: new[] { "DepartmentName", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Designations_GradeId",
                table: "HR_Designations",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Designations_NonClusteredIndex",
                table: "HR_Designations",
                columns: new[] { "DesignationName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Districts_DivisionId",
                table: "HR_Districts",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Districts_NonClusteredIndex",
                table: "HR_Districts",
                columns: new[] { "DistrictName", "DistrictCode", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Divisions_CountryId",
                table: "HR_Divisions",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Divisions_NonClusteredIndex",
                table: "HR_Divisions",
                columns: new[] { "DivisionName", "DivisionCode", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EducationalDegrees_LevelOfEducationId",
                table: "HR_EducationalDegrees",
                column: "LevelOfEducationId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EducationalDegrees_NonClusteredIndex",
                table: "HR_EducationalDegrees",
                columns: new[] { "DegreeName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmailSendingConfiguration_NonClusteredIndex",
                table: "HR_EmailSendingConfiguration",
                columns: new[] { "ModuleName", "EmailStage", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_FunctionalDivision_NonClusteredIndex",
                table: "HR_EmployeeAccountInfo",
                columns: new[] { "EmployeeId", "PaymentMode", "BankId", "AgentName", "AccountNo", "StateStatus", "IsActive", "IsApproved", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeConfirmationProposal_NonClusteredIndex",
                table: "HR_EmployeeConfirmationProposal",
                columns: new[] { "ConfirmationDate", "EffectiveDate", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeCurrentSalaryBreakDown_NonClusteredIndex",
                table: "HR_EmployeeCurrentSalaryBreakDown",
                columns: new[] { "EmployeeId", "AllowanceNameId", "IsActive", "Flag", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeDetail_NonClusteredIndex",
                table: "HR_EmployeeDetail",
                columns: new[] { "EmployeeId", "Gender", "Religion", "MaritalStatus", "IsResidential", "IsPhysicallyDisabled", "IsFreedomFighter", "IsMobility", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeDocument_NonClusteredIndex",
                table: "HR_EmployeeDocument",
                columns: new[] { "EmployeeId", "DocumentName", "DocumentNumber", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeEducation_EmployeeId",
                table: "HR_EmployeeEducation",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeEducation_NonClusteredIndex",
                table: "HR_EmployeeEducation",
                columns: new[] { "LevelOfEducationId", "DegreeId", "Major", "InstitutionName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeExperience_EmployeeId",
                table: "HR_EmployeeExperience",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeExperience_NonClusteredIndex",
                table: "HR_EmployeeExperience",
                columns: new[] { "EmployeeCode", "ExCompanyname", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeFamilyInfo_EmployeeId",
                table: "HR_EmployeeFamilyInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeFamilyInfo_NonClusteredIndex",
                table: "HR_EmployeeFamilyInfo",
                columns: new[] { "Relatation", "Name", "Age", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeHierarchy_EmployeeId",
                table: "HR_EmployeeHierarchy",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeHierarchy_NonClusteredIndex",
                table: "HR_EmployeeHierarchy",
                columns: new[] { "SupervisorId", "LineManagerId", "ManagerId", "HeadOfDepartmentId", "HRAuthorityId", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeHistory_NonClusteredIndex",
                table: "HR_EmployeeHistory",
                columns: new[] { "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeInformation_NonClusteredIndex",
                table: "HR_EmployeeInformation",
                columns: new[] { "EmployeeCode", "FirstName", "MiddleName", "LastName", "FullName", "GradeId", "DesignationId", "InternalDesignationId", "DepartmentId", "SectionId", "SubSectionId", "UnitId", "CostCenterId", "OfficeEmail", "AppointmentDate", "DateOfJoining", "DateOfConfirmation", "StateStatus", "IsActive", "IsPFMember", "IsConfirmed", "IsFBActive", "TerminationDate", "TerminationStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeNomineeInfo_EmployeeId",
                table: "HR_EmployeeNomineeInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeNomineeInfo_NonClusteredIndex",
                table: "HR_EmployeeNomineeInfo",
                columns: new[] { "Relation", "DocumentType", "DocumentNumber", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeePFActivation_NonClusteredIndex",
                table: "HR_EmployeePFActivation",
                columns: new[] { "PFBasedAmount", "PFEffectiveDate", "PFActivationDate", "StateStatus", "IsApproved", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeProbationaryExtensionProposal_NonClusteredIndex",
                table: "HR_EmployeeProbationaryExtensionProposal",
                columns: new[] { "ExtensionFrom", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeePromotionProposal_NonClusteredIndex",
                table: "HR_EmployeePromotionProposal",
                columns: new[] { "Head", "Flag", "IsActive", "StateStatus", "EffectiveDate", "ActiveDate", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeRelativesInfo_EmployeeId",
                table: "HR_EmployeeRelativesInfo",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeSkill_EmployeeId",
                table: "HR_EmployeeSkill",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeSkill_NonClusteredIndex",
                table: "HR_EmployeeSkill",
                columns: new[] { "Organization", "TopicCovers", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeTransferProposal_NonClusteredIndex",
                table: "HR_EmployeeTransferProposal",
                columns: new[] { "Head", "Flag", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmployeeType_NonClusteredIndex",
                table: "HR_EmployeeType",
                columns: new[] { "EmployeeTypeName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_EmploymentConfirmationOrProbotion_NonClusteredIndex",
                table: "HR_EmploymentConfirmationOrProbotion",
                columns: new[] { "EmployeeId", "Type", "Flag", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_ExceptionLogger_CompanyId",
                table: "HR_ExceptionLogger",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_FunctionalDivision_NonClusteredIndex",
                table: "HR_FunctionalDivision",
                columns: new[] { "FunctionalDivisionName", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Grades_NonClusteredIndex",
                table: "HR_Grades",
                columns: new[] { "GradeName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_JobCategory_NonClusteredIndex",
                table: "HR_JobCategory",
                columns: new[] { "JobCategoryName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Jobtypes_NonClusteredIndex",
                table: "HR_Jobtypes",
                columns: new[] { "JobTypeName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_LevelOfEducation_NonClusteredIndex",
                table: "HR_LevelOfEducation",
                columns: new[] { "Name", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Levels_NonClusteredIndex",
                table: "HR_Levels",
                columns: new[] { "LevelName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Line_NonClusteredIndex",
                table: "HR_Line",
                columns: new[] { "LineName", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Locations_NonClusteredIndex",
                table: "HR_Locations",
                columns: new[] { "LocationName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_PoliceStations_DistrictId",
                table: "HR_PoliceStations",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_PoliceStations_NonClusteredIndex",
                table: "HR_PoliceStations",
                columns: new[] { "PoliceStationName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_Sections_NonClusteredIndex",
                table: "HR_Sections",
                columns: new[] { "SectionName", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_SubSections_NonClusteredIndex",
                table: "HR_SubSections",
                columns: new[] { "SubSectionName", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_HR_SubSections_SectionId",
                table: "HR_SubSections",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_Units_NonClusteredIndex",
                table: "HR_Units",
                columns: new[] { "UnitName", "IsActive", "CompanyId", "OrganizationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_ActivityLogger");

            migrationBuilder.DropTable(
                name: "HR_BankBranches");

            migrationBuilder.DropTable(
                name: "HR_CompanyAccountInfo");

            migrationBuilder.DropTable(
                name: "HR_CompanyEventsCalendar");

            migrationBuilder.DropTable(
                name: "HR_ContractualEmployment");

            migrationBuilder.DropTable(
                name: "HR_Costcenter");

            migrationBuilder.DropTable(
                name: "HR_DataLabel");

            migrationBuilder.DropTable(
                name: "HR_DepartmentalDivision");

            migrationBuilder.DropTable(
                name: "HR_Designations");

            migrationBuilder.DropTable(
                name: "HR_DiscontinuedEmployee");

            migrationBuilder.DropTable(
                name: "HR_EducationalDegrees");

            migrationBuilder.DropTable(
                name: "HR_EmailSendingConfiguration");

            migrationBuilder.DropTable(
                name: "HR_EmployeeAccountInfo");

            migrationBuilder.DropTable(
                name: "HR_EmployeeConfirmationProposal");

            migrationBuilder.DropTable(
                name: "HR_EmployeeCurrentSalaryBreakDown");

            migrationBuilder.DropTable(
                name: "HR_EmployeeDetail");

            migrationBuilder.DropTable(
                name: "HR_EmployeeDocument");

            migrationBuilder.DropTable(
                name: "HR_EmployeeEducation");

            migrationBuilder.DropTable(
                name: "HR_EmployeeExperience");

            migrationBuilder.DropTable(
                name: "HR_EmployeeFamilyInfo");

            migrationBuilder.DropTable(
                name: "HR_EmployeeHierarchy");

            migrationBuilder.DropTable(
                name: "HR_EmployeeHistory");

            migrationBuilder.DropTable(
                name: "HR_EmployeeNomineeInfo");

            migrationBuilder.DropTable(
                name: "HR_EmployeePFActivation");

            migrationBuilder.DropTable(
                name: "HR_EmployeeProbationaryExtensionProposal");

            migrationBuilder.DropTable(
                name: "HR_EmployeePromotionProposal");

            migrationBuilder.DropTable(
                name: "HR_EmployeeRelativesInfo");

            migrationBuilder.DropTable(
                name: "HR_EmployeeSkill");

            migrationBuilder.DropTable(
                name: "HR_EmployeeTransferProposal");

            migrationBuilder.DropTable(
                name: "HR_EmployeeType");

            migrationBuilder.DropTable(
                name: "HR_EmploymentConfirmationOrProbotion");

            migrationBuilder.DropTable(
                name: "HR_ExceptionLogger");

            migrationBuilder.DropTable(
                name: "HR_FunctionalDivision");

            migrationBuilder.DropTable(
                name: "HR_JobCategory");

            migrationBuilder.DropTable(
                name: "HR_Jobtypes");

            migrationBuilder.DropTable(
                name: "HR_Levels");

            migrationBuilder.DropTable(
                name: "HR_Line");

            migrationBuilder.DropTable(
                name: "HR_Locations");

            migrationBuilder.DropTable(
                name: "HR_PoliceStations");

            migrationBuilder.DropTable(
                name: "HR_SubSections");

            migrationBuilder.DropTable(
                name: "HR_Units");

            migrationBuilder.DropTable(
                name: "HR_Banks");

            migrationBuilder.DropTable(
                name: "HR_CompanyEvents");

            migrationBuilder.DropTable(
                name: "HR_Departments");

            migrationBuilder.DropTable(
                name: "HR_Grades");

            migrationBuilder.DropTable(
                name: "HR_LevelOfEducation");

            migrationBuilder.DropTable(
                name: "HR_EmployeeInformation");

            migrationBuilder.DropTable(
                name: "HR_Districts");

            migrationBuilder.DropTable(
                name: "HR_Sections");

            migrationBuilder.DropTable(
                name: "HR_Divisions");

            migrationBuilder.DropTable(
                name: "HR_Countries");
        }
    }
}
