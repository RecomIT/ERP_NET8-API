using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class PayrollModuleInit25May2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_AnonymousEmployeeInfo",
                columns: table => new
                {
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSectionId = table.Column<long>(type: "bigint", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    ZoneId = table.Column<long>(type: "bigint", nullable: true),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true),
                    WorkShiftId = table.Column<long>(type: "bigint", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "date", nullable: true),
                    ContractEndStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContractEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    PayrollCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReferenceNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NIDNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    NIDFilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DivisionId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    TerminationStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TerminationDate = table.Column<DateTime>(type: "date", nullable: true),
                    FatherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SpouseName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PersonalMobileNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PersonalEmailAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Feet = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Inch = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    BloodGroup = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    PresentAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PermanentAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EmergencyContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RelationWithEmergencyContactPerson = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmergencyContactNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    EmergencyContactAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsResidential = table.Column<bool>(type: "bit", nullable: true),
                    IsPhysicallyDisabled = table.Column<bool>(type: "bit", nullable: true),
                    IsFreedomFighter = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_HR_AnonymousEmployeeInfo", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "HR_InternalDesignations",
                columns: table => new
                {
                    InternalDesignationId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternalDesignationName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_HR_InternalDesignations", x => x.InternalDesignationId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_ActualTaxDeduction",
                columns: table => new
                {
                    ActualTaxDeductionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ActualTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SystemFileName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_ActualTaxDeduction", x => x.ActualTaxDeductionId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_AllowanceConfiguration",
                columns: table => new
                {
                    ConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsMonthly = table.Column<bool>(type: "bit", nullable: true),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: true),
                    IsIndividual = table.Column<bool>(type: "bit", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsConfirmationRequired = table.Column<bool>(type: "bit", nullable: true),
                    DepandsOnWorkingHour = table.Column<bool>(type: "bit", nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeactivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsPerquisite = table.Column<bool>(type: "bit", nullable: true),
                    TaxConditionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProjectRestYear = table.Column<bool>(type: "bit", nullable: true),
                    IsTaxDistributed = table.Column<bool>(type: "bit", nullable: true),
                    OnceOffDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsOnceOffTax = table.Column<bool>(type: "bit", nullable: true),
                    FlatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptedPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_AllowanceConfiguration", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_AllowanceHead",
                columns: table => new
                {
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowanceHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AllowanceHeadCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AllowanceHeadNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_AllowanceHead", x => x.AllowanceHeadId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_Bonus",
                columns: table => new
                {
                    BonusId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonusName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BonusState = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ActivatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeactivatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DeactivationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Payroll_Bonus", x => x.BonusId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_CashSalaryHead",
                columns: table => new
                {
                    CashSalaryHeadId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CashSalaryHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CashSalaryHeadCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CashSalaryHeadNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_CashSalaryHead", x => x.CashSalaryHeadId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_CashSalaryProcess",
                columns: table => new
                {
                    CashSalaryProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "date", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_CashSalaryProcess", x => x.CashSalaryProcessId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_ConditionalDepositAllowanceConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceLength = table.Column<int>(type: "int", nullable: true),
                    ServiceLengthUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Citizen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhysicalCondition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsVisibleInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    IsVisibleInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    DepositType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsTreatAsSalaryBreakdownComponent = table.Column<bool>(type: "bit", nullable: false),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ActivationFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ActivationTo = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_ConditionalDepositAllowanceConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_ConditionalProjectedPayment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: true),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: true),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Citizen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhysicalCondition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    ShowInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    ShowInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    HasEmployeeTypes = table.Column<bool>(type: "bit", nullable: true),
                    HasEmployee = table.Column<bool>(type: "bit", nullable: true),
                    HasExcludeEmployee = table.Column<bool>(type: "bit", nullable: true),
                    HasBranch = table.Column<bool>(type: "bit", nullable: true),
                    HasDivision = table.Column<bool>(type: "bit", nullable: true),
                    HasGrade = table.Column<bool>(type: "bit", nullable: true),
                    HasInternalDesignation = table.Column<bool>(type: "bit", nullable: true),
                    HasDesignation = table.Column<bool>(type: "bit", nullable: true),
                    HasDepartment = table.Column<bool>(type: "bit", nullable: true),
                    HasSection = table.Column<bool>(type: "bit", nullable: true),
                    HasSubSection = table.Column<bool>(type: "bit", nullable: true),
                    HasUnit = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_ConditionalProjectedPayment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_DeductionConfiguration",
                columns: table => new
                {
                    ConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsMonthly = table.Column<bool>(type: "bit", nullable: true),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: true),
                    IsIndividual = table.Column<bool>(type: "bit", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsConfirmationRequired = table.Column<bool>(type: "bit", nullable: true),
                    DepandsOnWorkingHour = table.Column<bool>(type: "bit", nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeactivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    ProjectRestYear = table.Column<bool>(type: "bit", nullable: true),
                    OnceOffDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsOnceOffTax = table.Column<bool>(type: "bit", nullable: true),
                    FlatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptedPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_DeductionConfiguration", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_DeductionHead",
                columns: table => new
                {
                    DeductionHeadId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeductionHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeductionHeadCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DeductionHeadNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_DeductionHead", x => x.DeductionHeadId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_DepositAllowanceHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    DepositMonth = table.Column<int>(type: "int", nullable: true),
                    DepositYear = table.Column<int>(type: "int", nullable: true),
                    PayableDays = table.Column<short>(type: "smallint", nullable: true),
                    DepositDate = table.Column<DateTime>(type: "date", nullable: true),
                    BaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Arrear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncomingFlag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeDepositAllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
                    ConditionalDepositAllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_DepositAllowanceHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_DepositAllowancePaymentHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMonth = table.Column<int>(type: "int", nullable: false),
                    PaymentYear = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsVisibleInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    IsVisibleInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    PaymentApproach = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentBeMade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    ProposalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncomingFlag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeDepositAllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
                    ConditionalDepositAllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_DepositAllowancePaymentHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeBonusTaxProcess",
                columns: table => new
                {
                    EmployeeBonusTaxProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    BonusId = table.Column<long>(type: "bigint", nullable: false),
                    BonusConfigId = table.Column<long>(type: "bigint", nullable: false),
                    BonusProcessId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    BonusMonth = table.Column<short>(type: "smallint", nullable: false),
                    BonusYear = table.Column<short>(type: "smallint", nullable: false),
                    RemainMonth = table.Column<short>(type: "smallint", nullable: false),
                    PFContributionBothPart = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalTaxPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvestmentRebateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualInvestmentMade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AITAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxReturnAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExcessTaxPaidRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidTotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BonusProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_EmployeeBonusTaxProcess", x => x.EmployeeBonusTaxProcessId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeBonusTaxProcessDetail",
                columns: table => new
                {
                    EmployeeBonusTaxProcessDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BonusId = table.Column<long>(type: "bigint", nullable: false),
                    BonusConfigId = table.Column<long>(type: "bigint", nullable: false),
                    BonusProcessId = table.Column<long>(type: "bigint", nullable: false),
                    BonusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxItem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TillDateIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentMonthIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectedIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessExempted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPerquisite = table.Column<bool>(type: "bit", nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    BonusMonth = table.Column<short>(type: "smallint", nullable: false),
                    BonusYear = table.Column<short>(type: "smallint", nullable: false),
                    AllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeBonusTaxProcessDetail", x => x.EmployeeBonusTaxProcessDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeBonusTaxProcessSlab",
                columns: table => new
                {
                    EmployeeBonusTaxProcessSlabId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    BonusId = table.Column<long>(type: "bigint", nullable: false),
                    BonusConfigId = table.Column<long>(type: "bigint", nullable: false),
                    BonusProcessId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    BonusMonth = table.Column<short>(type: "smallint", nullable: false),
                    BonusYear = table.Column<short>(type: "smallint", nullable: false),
                    IncomeTaxSlabId = table.Column<long>(type: "bigint", nullable: true),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlabPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxLiability = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BonusDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeBonusTaxProcessSlab", x => x.EmployeeBonusTaxProcessSlabId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeDepositAllowanceConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    IsVisibleInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    IsVisibleInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    DepositType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsTreatAsSalaryBreakdownComponent = table.Column<bool>(type: "bit", nullable: false),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ActivationFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ActivationTo = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeDepositAllowanceConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeExcludedFromBonus",
                columns: table => new
                {
                    ExcludeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    BonusId = table.Column<long>(type: "bigint", nullable: false),
                    BonusConfigId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_EmployeeExcludedFromBonus", x => x.ExcludeId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeFreeCar",
                columns: table => new
                {
                    FreeCarId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsProjected = table.Column<bool>(type: "bit", nullable: true),
                    CarModelNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CCOfCar = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: true),
                    EndDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeFreeCar", x => x.FreeCarId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeProjectedAllowanceProcessInfo",
                columns: table => new
                {
                    ProjectedAllowanceProcessInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    HeadCount = table.Column<int>(type: "int", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: false),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    ShowInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    ShowInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    WithCOC = table.Column<bool>(type: "bit", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    PayableDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeProjectedAllowanceProcessInfo", x => x.ProjectedAllowanceProcessInfoId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeTaxProcess",
                columns: table => new
                {
                    TaxProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    RemainMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    TotalTillMonthAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCurrentMonthAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalProjectedAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLessExemptedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalGrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrossTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptionAmountOnAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFContributionBothPart = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalTaxPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvestmentRebateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualInvestmentMade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AITAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxReturnAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExcessTaxPaidRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidTotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupplementaryOnceffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFExemption = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalIncomeAfterPFExemption = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxProcessUniqId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActualTaxDeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentMonthProjectedTaxDeducted = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentMonthOnceOffTaxDeducted = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YTDLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectionLabel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TillProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TillOnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeTaxProcess", x => x.TaxProcessId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeTaxProcessDetail",
                columns: table => new
                {
                    TaxProcessDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxProcessId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BonusId = table.Column<long>(type: "bigint", nullable: false),
                    BonusName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxItem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TillDateIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentMonthIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectedIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessExempted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPerquisite = table.Column<bool>(type: "bit", nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    AllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeTaxProcessDetail", x => x.TaxProcessDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeTaxProcessSlab",
                columns: table => new
                {
                    EmployeeTaxProcessSlabId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    IncomeTaxSlabId = table.Column<long>(type: "bigint", nullable: true),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlabPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxLiability = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaxProcessId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_EmployeeTaxProcessSlab", x => x.EmployeeTaxProcessSlabId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeTaxReturnSubmission",
                columns: table => new
                {
                    TaxSubmissionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaxZone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TaxCircle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SubmissionDate = table.Column<DateTime>(type: "date", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeTaxReturnSubmission", x => x.TaxSubmissionId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeTaxZone",
                columns: table => new
                {
                    EmployeeTaxZoneId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    Taxzone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinimumTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    ActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    InActiveDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_Payroll_EmployeeTaxZone", x => x.EmployeeTaxZoneId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeYearlyInvestment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    InvestmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_EmployeeYearlyInvestment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_FiscalYear",
                columns: table => new
                {
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearRange = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AssesmentYear = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FiscalYearFrom = table.Column<DateTime>(type: "date", nullable: true),
                    FiscalYearTo = table.Column<DateTime>(type: "date", nullable: true),
                    AuthorizedBy = table.Column<long>(type: "bigint", nullable: true),
                    AuthorizerDesignationId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorizedSignForTax = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_FiscalYear", x => x.FiscalYearId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_IncomeTaxSetting",
                columns: table => new
                {
                    IncomeTaxSettingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    AssessmentYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaxTaxAge = table.Column<short>(type: "smallint", nullable: true),
                    MinTaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsFlatRebate = table.Column<bool>(type: "bit", nullable: false),
                    ExemptionAmountOfAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptionPercentageOfAnnualIncome = table.Column<decimal>(type: "decimal(18,11)", nullable: true),
                    FreeCarCCMinimumLimit = table.Column<int>(type: "int", nullable: true),
                    FreeCarMinTaxableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FreeCarMaxTaxableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyTaxDeductionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CalculateTaxOnArrearAmount = table.Column<bool>(type: "bit", nullable: true),
                    PFBothPartExemption = table.Column<bool>(type: "bit", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_IncomeTaxSetting", x => x.IncomeTaxSettingId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_IncomeTaxSlab",
                columns: table => new
                {
                    IncomeTaxSlabId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    AssessmentYear = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlabMininumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SlabMaximumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SlabPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_IncomeTaxSlab", x => x.IncomeTaxSlabId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyAlllowanceConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ServiceLength = table.Column<int>(type: "int", nullable: true),
                    ServiceLengthUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Citizen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhysicalCondition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PFEmployee = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    HasEmployeeTypes = table.Column<bool>(type: "bit", nullable: true),
                    HasEmployee = table.Column<bool>(type: "bit", nullable: true),
                    HasBranch = table.Column<bool>(type: "bit", nullable: true),
                    HasDivision = table.Column<bool>(type: "bit", nullable: true),
                    HasGrade = table.Column<bool>(type: "bit", nullable: true),
                    HasInternalDesignation = table.Column<bool>(type: "bit", nullable: true),
                    HasDesignation = table.Column<bool>(type: "bit", nullable: true),
                    HasDepartment = table.Column<bool>(type: "bit", nullable: true),
                    HasSection = table.Column<bool>(type: "bit", nullable: true),
                    HasSubSection = table.Column<bool>(type: "bit", nullable: true),
                    HasUnit = table.Column<bool>(type: "bit", nullable: true),
                    HasExcludeEmployee = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ActivationFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ActivationTo = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_MonthlyAlllowanceConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyIncentiveProcess",
                columns: table => new
                {
                    MonthlyIncentiveProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IncentiveMonth = table.Column<short>(type: "smallint", nullable: false),
                    IncentiveYear = table.Column<short>(type: "smallint", nullable: false),
                    MonthlyIncentiveNoId = table.Column<long>(type: "bigint", nullable: true),
                    MonthlyIncentiveName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ProcessDate = table.Column<DateTime>(type: "date", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    PayableDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_MonthlyIncentiveProcess", x => x.MonthlyIncentiveProcessId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_PeriodicallyVariableAllowanceInfo",
                columns: table => new
                {
                    PeriodicallyVariableAllowanceInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpecifyFor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AmountBaseOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DurationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    CalculateProratedAmount = table.Column<bool>(type: "bit", nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Citizen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_PeriodicallyVariableAllowanceInfo", x => x.PeriodicallyVariableAllowanceInfoId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_PrincipleAmountInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncomingFlag = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    VariableType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IncomingId = table.Column<long>(type: "bigint", nullable: false),
                    VariableId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_PrincipleAmountInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_QuarterlyIncentiveProcess",
                columns: table => new
                {
                    QuarterlyIncentiveProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IncentiveQuarterNoId = table.Column<long>(type: "bigint", nullable: true),
                    IncentiveQuarterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IncentiveYear = table.Column<short>(type: "smallint", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    ProcessDate = table.Column<DateTime>(type: "date", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    PayableDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_QuarterlyIncentiveProcess", x => x.QuarterlyIncentiveProcessId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_RecipientsofServiceAnniversaryAllowance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMonth = table.Column<int>(type: "int", nullable: false),
                    PaymentYear = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    IsVisibleInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    IsVisibleInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceAnniversaryAllowanceId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_RecipientsofServiceAnniversaryAllowance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryAllowance",
                columns: table => new
                {
                    SalaryAllowanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculationForDays = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeriodicallyAllowanceId = table.Column<long>(type: "bigint", nullable: true),
                    PeriodicallyAllowanceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthlyAllowanceId = table.Column<long>(type: "bigint", nullable: true),
                    MonthlyAllowanceIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryReviewInfoId = table.Column<long>(type: "bigint", nullable: true),
                    BaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SalaryProcessUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryAllowance", x => x.SalaryAllowanceId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryAllowanceAdjustment",
                columns: table => new
                {
                    AllowanceAdjustmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculationForDays = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustmentMonth = table.Column<short>(type: "smallint", nullable: false),
                    AdjustmentYear = table.Column<short>(type: "smallint", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SalaryProcessUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalaryHoldId = table.Column<long>(type: "bigint", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryAllowanceAdjustment", x => x.AllowanceAdjustmentId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryAllowanceArrear",
                columns: table => new
                {
                    SalaryAllowanceArrearId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculationForDays = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArrearMonth = table.Column<short>(type: "smallint", nullable: false),
                    ArrearYear = table.Column<short>(type: "smallint", nullable: false),
                    ArrearFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ArrearTo = table.Column<DateTime>(type: "date", nullable: true),
                    Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SalaryProcessUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalaryReviewInfoId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryAllowanceArrear", x => x.SalaryAllowanceArrearId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryAllowanceConfigurationInfo",
                columns: table => new
                {
                    SalaryAllowanceConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigCategory = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BaseType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CheckedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CheckedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_SalaryAllowanceConfigurationInfo", x => x.SalaryAllowanceConfigId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryDeduction",
                columns: table => new
                {
                    SalaryDeductionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculationForDays = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PeriodicallyDeductionId = table.Column<long>(type: "bigint", nullable: true),
                    PeriodicallyDeductionIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MonthlyDeductionId = table.Column<long>(type: "bigint", nullable: true),
                    MonthlyDeductionIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    SalaryProcessUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryDeduction", x => x.SalaryDeductionId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryDeductionAdjustment",
                columns: table => new
                {
                    DeductionAdjustmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculationForDays = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustmentMonth = table.Column<short>(type: "smallint", nullable: false),
                    AdjustmentYear = table.Column<short>(type: "smallint", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryDeductionAdjustment", x => x.DeductionAdjustmentId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryHold",
                columns: table => new
                {
                    SalaryHoldId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    IsHolded = table.Column<bool>(type: "bit", nullable: true),
                    HoldReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    HoldFrom = table.Column<DateTime>(type: "date", nullable: true),
                    HoldTo = table.Column<DateTime>(type: "date", nullable: true),
                    WithSalary = table.Column<bool>(type: "bit", nullable: true),
                    WithoutSalary = table.Column<bool>(type: "bit", nullable: true),
                    PFContinue = table.Column<bool>(type: "bit", nullable: true),
                    GFContinue = table.Column<bool>(type: "bit", nullable: true),
                    UnholdDate = table.Column<DateTime>(type: "date", nullable: true),
                    UnholdReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeResignationId = table.Column<long>(type: "bigint", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryHold", x => x.SalaryHoldId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryProcess",
                columns: table => new
                {
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcessType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalaryProcessUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DocFileNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "date", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    DivisionId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: false),
                    TotalEmployees = table.Column<int>(type: "int", nullable: false),
                    TotalAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArrearAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAllowanceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPFArrear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArrearDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMonthlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxDeductedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalGrossPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalHoldDays = table.Column<int>(type: "int", nullable: true),
                    TotalHoldAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalUnholdDays = table.Column<int>(type: "int", nullable: true),
                    TotalUnholdAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CompanyAccountId = table.Column<long>(type: "bigint", nullable: true),
                    RegularNetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegularNetPayLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegularNetPayDifferenceWithLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContractualNetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContractualNetPayLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContractualNetPayDifferenceWithLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegularAndContractualNetPayLastMonthTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayRegular = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayRegularLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayRegularDifferenceWithLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayContractual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayContractualLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayContractualDifferenceWithLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayRegularAndContractualTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegularAndContractualNetPayDifferencWithLastMonthTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResignedNetPayRegular = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResignedNetPayContractual = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResignedNetPayRegularAndContractualTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegularNetPayHeadCountLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ContractualNetPayHeadCountLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    RegularAndContractualNetPayHeadCountTotalLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayRegularHeadCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayContractualHeadCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayRegularAndContractualHeadCountTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResignedNetPayRegularHeadCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResignedNetPayContractualHeadCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ResignedNetPayRegularAndContractualHeadCountTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashSalaryLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashSalaryDifferenceWithLastMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashSalaryNewJoined = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashSalaryResigned = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewJoinerNetPayRegularAndContractualDifferencWithLastMonthTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryProcess", x => x.SalaryProcessId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryProcessSummery",
                columns: table => new
                {
                    SalaryProcessSummeryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HeadType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HeadId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    TotalAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArrearAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAllowanceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PFArrear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArrearDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMonthlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxDeductedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_SalaryProcessSummery", x => x.SalaryProcessSummeryId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryReviewInfo",
                columns: table => new
                {
                    SalaryReviewInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    InternalDesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    MonthlyCTC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyPF = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyFB = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BaseType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CurrentSalaryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalaryBaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousSalaryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CTCAmountWithoutFestivalBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryAllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryConfigCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IncrementReason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    IsAutoCalculate = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeactivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsArrearCalculated = table.Column<bool>(type: "bit", nullable: false),
                    ArrearCalculatedDate = table.Column<DateTime>(type: "date", nullable: true),
                    PreviousReviewId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryReviewInfo", x => x.SalaryReviewInfoId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_ServiceAnniversaryAllowance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    ConsiderPaymentMonthLastDate = table.Column<bool>(type: "bit", nullable: true),
                    CutOffDay = table.Column<int>(type: "int", nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Religion = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaritalStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Citizen = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhysicalCondition = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsVisibleInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    IsVisibleInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ActivationFrom = table.Column<DateTime>(type: "date", nullable: true),
                    ActivationTo = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_ServiceAnniversaryAllowance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SupplementaryPaymentAmount",
                columns: table => new
                {
                    PaymentAmountId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: false),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: false),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentProcessInfoId = table.Column<long>(type: "bigint", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeAccountId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeWalletId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BankBranchName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WithCOC = table.Column<bool>(type: "bit", nullable: true),
                    COCInWalletTransfer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayrollCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PaymentHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    Adjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SupplementaryPaymentAmount", x => x.PaymentAmountId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SupplementaryPaymentProcessInfo",
                columns: table => new
                {
                    PaymentProcessInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    TotalEmployees = table.Column<int>(type: "int", nullable: false),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: false),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: false),
                    CutOffDate = table.Column<DateTime>(type: "date", nullable: true),
                    EffectOnPayslip = table.Column<bool>(type: "bit", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOnceOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    ProcessType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ShowInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    ShowInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    PayableDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SupplementaryPaymentProcessInfo", x => x.PaymentProcessInfoId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SupplementaryPaymentTaxDetail",
                columns: table => new
                {
                    SupplementaryPaymentTaxDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTaxInfoId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceHeadName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TaxItem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TillDateIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentMonthIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectedIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LessExempted = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPerquisite = table.Column<bool>(type: "bit", nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: false),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: false),
                    AllowanceConfigId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SupplementaryPaymentTaxDetail", x => x.SupplementaryPaymentTaxDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SupplementaryPaymentTaxInfo",
                columns: table => new
                {
                    PaymentTaxInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentProcessInfoId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentAmountId = table.Column<long>(type: "bigint", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    RemainMonth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: false),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: false),
                    TotalTillMonthAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCurrentMonthAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalProjectedAllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLessExemptedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalGrossAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    GrossTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExemptionAmountOnAnnualIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFContributionBothPart = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherInvestment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalTaxPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InvestmentRebateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualInvestmentMade = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AITAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxReturnAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExcessTaxPaidRefundAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    YearlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaidTotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MonthlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProjectionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OnceOffAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ArrearAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PFExemption = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalIncomeAfterPFExemption = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxProcessUniqId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActualTaxDeductionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SupplementaryPaymentTaxInfo", x => x.PaymentTaxInfoId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SupplementaryPaymentTaxSlab",
                columns: table => new
                {
                    SupplementaryPaymentTaxSlabId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: false),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: false),
                    IncomeTaxSlabId = table.Column<long>(type: "bigint", nullable: true),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SlabPercentage = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    ParameterName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxableIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxLiability = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentTaxInfoId = table.Column<long>(type: "bigint", nullable: false),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    PayableDate = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SupplementaryPaymentTaxSlab", x => x.SupplementaryPaymentTaxSlabId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_TaxChallan",
                columns: table => new
                {
                    TaxChallanId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaxMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaxYear = table.Column<short>(type: "smallint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ChallanNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    ChallanDate = table.Column<DateTime>(type: "date", nullable: true),
                    DepositeBank = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DepositeBranch = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_TaxChallan", x => x.TaxChallanId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_TaxDocumentSubmission",
                columns: table => new
                {
                    SubmissionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    CertificateType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAuction = table.Column<bool>(type: "bit", nullable: false),
                    ActualFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RegSlNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NumberOfCar = table.Column<short>(type: "smallint", nullable: true),
                    TaxZone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxCircle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxUnit = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SubmissionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Payroll_TaxDocumentSubmission", x => x.SubmissionId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_TaxExemptionSetting",
                columns: table => new
                {
                    TaxExemptionSettingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Allowance = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxExemptionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BasedOfAllowance = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    MaxExemptionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TakeLowerAmount = table.Column<bool>(type: "bit", nullable: true),
                    UptoMaxAmount = table.Column<bool>(type: "bit", nullable: true),
                    UptoMaxPercentage = table.Column<bool>(type: "bit", nullable: true),
                    TakeMinAmount = table.Column<bool>(type: "bit", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IncomeTaxSettingId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_TaxExemptionSetting", x => x.TaxExemptionSettingId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_TaxInvestmentSetting",
                columns: table => new
                {
                    TaxInvestmentSettingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    ImpliedCondition = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MaxInvestmentPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RebateAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Operator = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MinRebate = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    MaxRebate = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    IsFlatRebate = table.Column<bool>(type: "bit", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IncomeTaxSettingId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_TaxInvestmentSetting", x => x.TaxInvestmentSettingId);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_UploadAllowance",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_UploadAllowance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_UploadDeduction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<short>(type: "smallint", nullable: false),
                    Year = table.Column<short>(type: "smallint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdjustmentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_UploadDeduction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_WalletPaymentConfiguration",
                columns: table => new
                {
                    WalletConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InternalDesignationId = table.Column<long>(type: "bigint", nullable: false),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    WalletFlatAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletTransferPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    COCInWalletTransferPercentage = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    COCInWalletTransfer = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    ActivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    DeactivationDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
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
                    table.PrimaryKey("PK_Payroll_WalletPaymentConfiguration", x => x.WalletConfigId);
                    table.ForeignKey(
                        name: "FK_Payroll_WalletPaymentConfiguration_HR_InternalDesignations_InternalDesignationId",
                        column: x => x.InternalDesignationId,
                        principalTable: "HR_InternalDesignations",
                        principalColumn: "InternalDesignationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_AllowanceName",
                columns: table => new
                {
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GLCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AllowanceNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowanceClientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowanceClientNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AllowanceDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AllowanceDescriptionInBengali = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AllowanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SerialNo = table.Column<long>(type: "bigint", nullable: true),
                    IsFixed = table.Column<bool>(type: "bit", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_AllowanceName", x => x.AllowanceNameId);
                    table.ForeignKey(
                        name: "FK_Payroll_AllowanceName_Payroll_AllowanceHead_AllowanceHeadId",
                        column: x => x.AllowanceHeadId,
                        principalTable: "Payroll_AllowanceHead",
                        principalColumn: "AllowanceHeadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_BonusConfig",
                columns: table => new
                {
                    BonusConfigId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonusConfigCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    IsReligious = table.Column<bool>(type: "bit", nullable: false),
                    ReligionName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsConfirmedEmployee = table.Column<bool>(type: "bit", nullable: true),
                    IsFestival = table.Column<bool>(type: "bit", nullable: true),
                    IsTaxable = table.Column<bool>(type: "bit", nullable: true),
                    TaxConditionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsPaymentProjected = table.Column<bool>(type: "bit", nullable: true),
                    IsTaxDistributed = table.Column<bool>(type: "bit", nullable: true),
                    IsOnceOff = table.Column<bool>(type: "bit", nullable: true),
                    IsExcludeFromSalary = table.Column<bool>(type: "bit", nullable: true),
                    IsTaxAdjustedWithSalary = table.Column<bool>(type: "bit", nullable: true),
                    IsPerquisite = table.Column<bool>(type: "bit", nullable: true),
                    BonusCount = table.Column<short>(type: "smallint", nullable: false),
                    BasedOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaximumAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BonusId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_BonusConfig", x => x.BonusConfigId);
                    table.ForeignKey(
                        name: "FK_Payroll_BonusConfig_Payroll_Bonus_BonusId",
                        column: x => x.BonusId,
                        principalTable: "Payroll_Bonus",
                        principalColumn: "BonusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_UploadCashSalary",
                columns: table => new
                {
                    UploadCashSalaryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PayrollCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsProcessed = table.Column<bool>(type: "bit", nullable: true),
                    CashSalaryProcessId = table.Column<long>(type: "bigint", nullable: true),
                    CashSalaryHeadId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_UploadCashSalary", x => x.UploadCashSalaryId);
                    table.ForeignKey(
                        name: "FK_Payroll_UploadCashSalary_Payroll_CashSalaryHead_CashSalaryHeadId",
                        column: x => x.CashSalaryHeadId,
                        principalTable: "Payroll_CashSalaryHead",
                        principalColumn: "CashSalaryHeadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_CashSalaryProcessDetail",
                columns: table => new
                {
                    CashSalaryDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalCashAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashPayWithoutCOC = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayrollCardTransfer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayrollCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CashSalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_CashSalaryProcessDetail", x => x.CashSalaryDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_CashSalaryProcessDetail_Payroll_CashSalaryProcess_CashSalaryProcessId",
                        column: x => x.CashSalaryProcessId,
                        principalTable: "Payroll_CashSalaryProcess",
                        principalColumn: "CashSalaryProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_ConditionalProjectedPaymentDetail",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: false),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: false),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InternalDesignationId = table.Column<long>(type: "bigint", nullable: true),
                    InternalDesignation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    Section = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SubsectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSection = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CostCenterCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    JobCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    JobCategory = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BankId = table.Column<long>(type: "bigint", nullable: true),
                    BankBranchId = table.Column<long>(type: "bigint", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BankTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    WalletAgent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletTransferAmont = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    COCInWalletTransfer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    ShowInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    ShowInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    ConditionalProjectedPaymentId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_ConditionalProjectedPaymentDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_ConditionalProjectedPaymentDetail_Payroll_ConditionalProjectedPayment_ConditionalProjectedPaymentId",
                        column: x => x.ConditionalProjectedPaymentId,
                        principalTable: "Payroll_ConditionalProjectedPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_ConditionalProjectedPaymentExcludeParameter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParameterId = table.Column<long>(type: "bigint", nullable: false),
                    ConditionalProjectedPaymentId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_ConditionalProjectedPaymentExcludeParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_ConditionalProjectedPaymentExcludeParameter_Payroll_ConditionalProjectedPayment_ConditionalProjectedPaymentId",
                        column: x => x.ConditionalProjectedPaymentId,
                        principalTable: "Payroll_ConditionalProjectedPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_ConditionalProjectedPaymentParameter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParameterId = table.Column<long>(type: "bigint", nullable: false),
                    ConditionalProjectedPaymentId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_ConditionalProjectedPaymentParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_ConditionalProjectedPaymentParameter_Payroll_ConditionalProjectedPayment_ConditionalProjectedPaymentId",
                        column: x => x.ConditionalProjectedPaymentId,
                        principalTable: "Payroll_ConditionalProjectedPayment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_DeductionName",
                columns: table => new
                {
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GLCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DeductionNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DeductionClientName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DeductionClientNameInBengali = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DeductionDescription = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DeductionDescriptionInBengali = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DeductionType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsFixed = table.Column<bool>(type: "bit", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    DeductionHeadId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_DeductionName", x => x.DeductionNameId);
                    table.ForeignKey(
                        name: "FK_Payroll_DeductionName_Payroll_DeductionHead_DeductionHeadId",
                        column: x => x.DeductionHeadId,
                        principalTable: "Payroll_DeductionHead",
                        principalColumn: "DeductionHeadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_EmployeeProjectedAllowance",
                columns: table => new
                {
                    ProjectedAllowanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectedAllowanceCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMonth = table.Column<short>(type: "smallint", nullable: true),
                    PaymentYear = table.Column<short>(type: "smallint", nullable: true),
                    AllowanceHeadId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    BaseOfPayment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayableAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisbursedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: true),
                    ShowInPayslip = table.Column<bool>(type: "bit", nullable: true),
                    ShowInSalarySheet = table.Column<bool>(type: "bit", nullable: true),
                    EmployeeAccountId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeWalletId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BankBranchName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WithCOC = table.Column<bool>(type: "bit", nullable: true),
                    COCInWalletTransfer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayrollCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    PaymentHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ProjectedAllowanceProcessInfoId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_EmployeeProjectedAllowance", x => x.ProjectedAllowanceId);
                    table.ForeignKey(
                        name: "FK_Payroll_EmployeeProjectedAllowance_Payroll_EmployeeProjectedAllowanceProcessInfo_ProjectedAllowanceProcessInfoId",
                        column: x => x.ProjectedAllowanceProcessInfoId,
                        principalTable: "Payroll_EmployeeProjectedAllowanceProcessInfo",
                        principalColumn: "ProjectedAllowanceProcessInfoId");
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyAllowanceConfigParameter",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Flag = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ParameterId = table.Column<long>(type: "bigint", nullable: false),
                    MonthlyAlllowanceConfigId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll_MonthlyAllowanceConfigParameter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_MonthlyAllowanceConfigParameter_Payroll_MonthlyAlllowanceConfig_MonthlyAlllowanceConfigId",
                        column: x => x.MonthlyAlllowanceConfigId,
                        principalTable: "Payroll_MonthlyAlllowanceConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyIncentiveProcessDetail",
                columns: table => new
                {
                    MonthlyIncentiveProcessDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthlyIncentiveProcessId = table.Column<long>(type: "bigint", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IncentiveMonth = table.Column<short>(type: "smallint", nullable: false),
                    IncentiveYear = table.Column<short>(type: "smallint", nullable: false),
                    MonthlyIncentiveNoId = table.Column<long>(type: "bigint", nullable: true),
                    MonthlyIncentiveName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AttendanceScore = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    CurrentBasic = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdjustedKpiPerformanceScore = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    ESSAURating = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AttendanceAdherenceQualityScore = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    EligibleIncentive = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalIncentive = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Adjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IncentiveTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WithCOC = table.Column<bool>(type: "bit", nullable: true),
                    COCInWalletTransfer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankBranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayrollCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Division = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PayableHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    PayableDate = table.Column<DateTime>(type: "date", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_MonthlyIncentiveProcessDetail", x => x.MonthlyIncentiveProcessDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_MonthlyIncentiveProcessDetail_Payroll_MonthlyIncentiveProcess_MonthlyIncentiveProcessId",
                        column: x => x.MonthlyIncentiveProcessId,
                        principalTable: "Payroll_MonthlyIncentiveProcess",
                        principalColumn: "MonthlyIncentiveProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_PeriodicallyVariableAllowanceDetail",
                columns: table => new
                {
                    PeriodicallyVariableAllowanceDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllownanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    SpecifyFor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    DurationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    AmountBaseOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    InActiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CalculatePoratedAmount = table.Column<bool>(type: "bit", nullable: true),
                    PeriodicallyVariableAllowanceInfoId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_PeriodicallyVariableAllowanceDetail", x => x.PeriodicallyVariableAllowanceDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_PeriodicallyVariableAllowanceDetail_Payroll_PeriodicallyVariableAllowanceInfo_PeriodicallyVariableAllowanceInfoId",
                        column: x => x.PeriodicallyVariableAllowanceInfoId,
                        principalTable: "Payroll_PeriodicallyVariableAllowanceInfo",
                        principalColumn: "PeriodicallyVariableAllowanceInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_QuarterlyIncentiveProcessDetail",
                columns: table => new
                {
                    QuarterlyIncentiveProcessDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    Division = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IncentiveYear = table.Column<short>(type: "smallint", nullable: false),
                    IncentiveQuarterNoId = table.Column<long>(type: "bigint", nullable: true),
                    IncentiveQuarterNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    CurrentBasic = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalKpiCompanyScore = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    TotalKpiDivisionalIndividualScore = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    TotalKpiAchievementScore = table.Column<decimal>(type: "decimal(18,6)", nullable: true),
                    EligibleQuarterlyIncentive = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalQuarterlyIncentive = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QuarterlyIncentiveTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BankBranchName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    RoutingNumber = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BankTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WithCOC = table.Column<bool>(type: "bit", nullable: true),
                    COCInWalletTransfer = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayrollCardNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    PayableHeadName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PayableMonth = table.Column<short>(type: "smallint", nullable: true),
                    PayableYear = table.Column<short>(type: "smallint", nullable: true),
                    Adjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayableDate = table.Column<DateTime>(type: "date", nullable: true),
                    QuarterlyIncentiveProcessId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_QuarterlyIncentiveProcessDetail", x => x.QuarterlyIncentiveProcessDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_QuarterlyIncentiveProcessDetail_Payroll_QuarterlyIncentiveProcess_QuarterlyIncentiveProcessId",
                        column: x => x.QuarterlyIncentiveProcessId,
                        principalTable: "Payroll_QuarterlyIncentiveProcess",
                        principalColumn: "QuarterlyIncentiveProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryAllowanceConfigurationDetails",
                columns: table => new
                {
                    SalaryAllowanceConfigDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AllowanceBase = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DependentAllowance = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MinAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AdditionalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPeriodically = table.Column<bool>(type: "bit", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SalaryAllowanceConfigId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_SalaryAllowanceConfigurationDetails", x => x.SalaryAllowanceConfigDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_SalaryAllowanceConfigurationDetails_Payroll_SalaryAllowanceConfigurationInfo_SalaryAllowanceConfigId",
                        column: x => x.SalaryAllowanceConfigId,
                        principalTable: "Payroll_SalaryAllowanceConfigurationInfo",
                        principalColumn: "SalaryAllowanceConfigId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryProcessDetail",
                columns: table => new
                {
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    Grade = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    Designation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    InternalDesignationId = table.Column<long>(type: "bigint", nullable: true),
                    InternalDesignation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    Section = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SubsectionId = table.Column<long>(type: "bigint", nullable: true),
                    SubSection = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CostCenterId = table.Column<long>(type: "bigint", nullable: true),
                    CostCenter = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CostCenterCode = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    Unit = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    JobTypeId = table.Column<long>(type: "bigint", nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    JobCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    JobCategory = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    EmployeeTypeId = table.Column<long>(type: "bigint", nullable: true),
                    EmployeeType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: false),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: false),
                    CalculationForDays = table.Column<decimal>(type: "decimal(4,2)", nullable: false),
                    CurrentBasic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentHouseRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentMedical = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentConveyance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThisMonthBasic = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThisMonthHouseRent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThisMonthMedical = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThisMonthConveyance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArrearAllowance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAllowanceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PFArrear = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalArrearDeduction = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectionTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalMonthlyTax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxDeductedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Payable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    BankId = table.Column<long>(type: "bigint", nullable: true),
                    BankBranchId = table.Column<long>(type: "bigint", nullable: true),
                    BankAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletAgent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WalletTransferAmont = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    COCInWalletTransfer = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankTransferAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DivisionId = table.Column<long>(type: "bigint", nullable: true),
                    IsHoldSalary = table.Column<bool>(type: "bit", nullable: true),
                    HoldDays = table.Column<short>(type: "smallint", nullable: true),
                    HoldAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UnholdDays = table.Column<short>(type: "smallint", nullable: true),
                    UnholdAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryProcessUniqId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SalaryReviewInfoIds = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    TotalPFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualPFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualPFArrear = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalActualPFAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BranchName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BreakdownWiseSalaryAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    LastSalaryReviewDate = table.Column<DateTime>(type: "date", nullable: true),
                    LastSalaryReviewId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_SalaryProcessDetail", x => x.SalaryProcessDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_SalaryProcessDetail_Payroll_SalaryProcess_SalaryProcessId",
                        column: x => x.SalaryProcessId,
                        principalTable: "Payroll_SalaryProcess",
                        principalColumn: "SalaryProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryReviewDetail",
                columns: table => new
                {
                    SalaryReviewDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalaryAllowanceConfigDetailId = table.Column<long>(type: "bigint", nullable: true),
                    AllowanceBase = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AllowancePercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AllowanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrentAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PreviousAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AdditionalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalaryReviewInfoId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_SalaryReviewDetail", x => x.SalaryReviewDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_SalaryReviewDetail_Payroll_SalaryReviewInfo_SalaryReviewInfoId",
                        column: x => x.SalaryReviewInfoId,
                        principalTable: "Payroll_SalaryReviewInfo",
                        principalColumn: "SalaryReviewInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyVariableAllowance",
                columns: table => new
                {
                    MonthlyVariableAllowanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryMonthYear = table.Column<DateTime>(type: "date", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: true),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: true),
                    AmountBaseOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_MonthlyVariableAllowance", x => x.MonthlyVariableAllowanceId);
                    table.ForeignKey(
                        name: "FK_Payroll_MonthlyVariableAllowance_Payroll_AllowanceName_AllowanceNameId",
                        column: x => x.AllowanceNameId,
                        principalTable: "Payroll_AllowanceName",
                        principalColumn: "AllowanceNameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_SalaryAllowanceArrearAdjustment",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    AllowanceNameId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: true),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: true),
                    SalaryDate = table.Column<DateTime>(type: "date", nullable: true),
                    CalculationForDays = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ArrearAdjustmentMonth = table.Column<short>(type: "smallint", nullable: true),
                    ArrearAdjustmentYear = table.Column<short>(type: "smallint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    SalaryProcessId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessDetailId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryProcessUniqueId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    table.PrimaryKey("PK_Payroll_SalaryAllowanceArrearAdjustment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payroll_SalaryAllowanceArrearAdjustment_Payroll_AllowanceName_AllowanceNameId",
                        column: x => x.AllowanceNameId,
                        principalTable: "Payroll_AllowanceName",
                        principalColumn: "AllowanceNameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_BonusProcess",
                columns: table => new
                {
                    BonusProcessId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BonusConfigId = table.Column<long>(type: "bigint", nullable: false),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    BonusMonth = table.Column<short>(type: "smallint", nullable: false),
                    BonusYear = table.Column<short>(type: "smallint", nullable: false),
                    IsDisbursed = table.Column<bool>(type: "bit", nullable: false),
                    TotalEmployees = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProcessDate = table.Column<DateTime>(type: "date", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    BonusId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Payroll_BonusProcess", x => x.BonusProcessId);
                    table.ForeignKey(
                        name: "FK_Payroll_BonusProcess_Payroll_BonusConfig_BonusConfigId",
                        column: x => x.BonusConfigId,
                        principalTable: "Payroll_BonusConfig",
                        principalColumn: "BonusConfigId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payroll_BonusProcess_Payroll_Bonus_BonusId",
                        column: x => x.BonusId,
                        principalTable: "Payroll_Bonus",
                        principalColumn: "BonusId");
                });

            migrationBuilder.CreateTable(
                name: "Payroll_MonthlyVariableDeduction",
                columns: table => new
                {
                    MonthlyVariableDeductionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    SalaryMonthYear = table.Column<DateTime>(type: "date", nullable: true),
                    SalaryMonth = table.Column<short>(type: "smallint", nullable: true),
                    SalaryYear = table.Column<short>(type: "smallint", nullable: true),
                    AmountBaseOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_MonthlyVariableDeduction", x => x.MonthlyVariableDeductionId);
                    table.ForeignKey(
                        name: "FK_Payroll_MonthlyVariableDeduction_Payroll_DeductionName_DeductionNameId",
                        column: x => x.DeductionNameId,
                        principalTable: "Payroll_DeductionName",
                        principalColumn: "DeductionNameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_PeriodicallyVariableDeductionInfo",
                columns: table => new
                {
                    PeriodicallyVariableDeductionInfoId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalaryVariableFor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AmountBaseOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DurationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_PeriodicallyVariableDeductionInfo", x => x.PeriodicallyVariableDeductionInfoId);
                    table.ForeignKey(
                        name: "FK_Payroll_PeriodicallyVariableDeductionInfo_Payroll_DeductionName_DeductionNameId",
                        column: x => x.DeductionNameId,
                        principalTable: "Payroll_DeductionName",
                        principalColumn: "DeductionNameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_BonusProcessDetail",
                columns: table => new
                {
                    BonusProcessDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OnceOffTax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BonusMonth = table.Column<short>(type: "smallint", nullable: false),
                    BonusYear = table.Column<short>(type: "smallint", nullable: false),
                    ProcessDate = table.Column<DateTime>(type: "date", nullable: true),
                    BonusProcessId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_BonusProcessDetail", x => x.BonusProcessDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_BonusProcessDetail_Payroll_BonusProcess_BonusProcessId",
                        column: x => x.BonusProcessId,
                        principalTable: "Payroll_BonusProcess",
                        principalColumn: "BonusProcessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payroll_PeriodicallyVariableDeductionDetail",
                columns: table => new
                {
                    PeriodicallyVariableDeductionDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeductionNameId = table.Column<long>(type: "bigint", nullable: false),
                    SalaryVariableFor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    DesignationId = table.Column<long>(type: "bigint", nullable: true),
                    GradeId = table.Column<long>(type: "bigint", nullable: true),
                    DurationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: true),
                    EffectiveFrom = table.Column<DateTime>(type: "date", nullable: true),
                    EffectiveTo = table.Column<DateTime>(type: "date", nullable: true),
                    AmountBaseOn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PrincipalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Percentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    PeriodicallyVariableDeductionInfoId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Payroll_PeriodicallyVariableDeductionDetail", x => x.PeriodicallyVariableDeductionDetailId);
                    table.ForeignKey(
                        name: "FK_Payroll_PeriodicallyVariableDeductionDetail_Payroll_PeriodicallyVariableDeductionInfo_PeriodicallyVariableDeductionInfoId",
                        column: x => x.PeriodicallyVariableDeductionInfoId,
                        principalTable: "Payroll_PeriodicallyVariableDeductionInfo",
                        principalColumn: "PeriodicallyVariableDeductionInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ActualTaxDeduction_NonClusteredIndex",
                table: "Payroll_ActualTaxDeduction",
                columns: new[] { "EmployeeId", "SalaryMonth", "SalaryYear", "FiscalYearId", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_AllowanceConfiguration_NonClusteredIndex",
                table: "Payroll_AllowanceConfiguration",
                columns: new[] { "AllowanceNameId", "StateStatus", "IsApproved", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_AllowanceHead_NonClusteredIndex",
                table: "Payroll_AllowanceHead",
                columns: new[] { "AllowanceHeadName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_AllowanceName_AllowanceHeadId",
                table: "Payroll_AllowanceName",
                column: "AllowanceHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_AllowanceName_NonClusteredIndex",
                table: "Payroll_AllowanceName",
                columns: new[] { "Name", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_Bonus_NonClusteredIndex",
                table: "Payroll_Bonus",
                columns: new[] { "BonusName", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BonusConfig_BonusId",
                table: "Payroll_BonusConfig",
                column: "BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BonusConfig_NonClusteredIndex",
                table: "Payroll_BonusConfig",
                columns: new[] { "BonusConfigCode", "FiscalYearId", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BonusProcess_BonusConfigId",
                table: "Payroll_BonusProcess",
                column: "BonusConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BonusProcess_BonusId",
                table: "Payroll_BonusProcess",
                column: "BonusId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BonusProcess_NonClusteredIndex",
                table: "Payroll_BonusProcess",
                columns: new[] { "FiscalYearId", "BonusMonth", "BonusYear", "BatchNo", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BonusProcessDetail_BonusProcessId",
                table: "Payroll_BonusProcessDetail",
                column: "BonusProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_BonusProcessDetail_NonClusteredIndex",
                table: "Payroll_BonusProcessDetail",
                columns: new[] { "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_CashSalaryProcessDetail_CashSalaryProcessId",
                table: "Payroll_CashSalaryProcessDetail",
                column: "CashSalaryProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalDepositAllowanceConfig_NonClusteredIndex",
                table: "Payroll_ConditionalDepositAllowanceConfig",
                columns: new[] { "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalProjectedPayment_NonClusteredIndex",
                table: "Payroll_ConditionalProjectedPayment",
                columns: new[] { "FiscalYearId", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalProjectedPaymentDetail_ConditionalProjectedPaymentId",
                table: "Payroll_ConditionalProjectedPaymentDetail",
                column: "ConditionalProjectedPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalProjectedPaymentDetail_NonClusteredIndex",
                table: "Payroll_ConditionalProjectedPaymentDetail",
                columns: new[] { "EmployeeId", "FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalProjectedPaymentExcludeParameter_ConditionalProjectedPaymentId",
                table: "Payroll_ConditionalProjectedPaymentExcludeParameter",
                column: "ConditionalProjectedPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalProjectedPaymentExcludeParameter_NonClusteredIndex",
                table: "Payroll_ConditionalProjectedPaymentExcludeParameter",
                columns: new[] { "Flag", "ParameterId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalProjectedPaymentParameter_ConditionalProjectedPaymentId",
                table: "Payroll_ConditionalProjectedPaymentParameter",
                column: "ConditionalProjectedPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ConditionalProjectedPaymentParameter_NonClusteredIndex",
                table: "Payroll_ConditionalProjectedPaymentParameter",
                columns: new[] { "Flag", "ParameterId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DeductionConfiguration_NonClusteredIndex",
                table: "Payroll_DeductionConfiguration",
                columns: new[] { "DeductionNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DeductionHead_NonClusteredIndex",
                table: "Payroll_DeductionHead",
                columns: new[] { "DeductionHeadName", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DeductionName_DeductionHeadId",
                table: "Payroll_DeductionName",
                column: "DeductionHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DeductionName_NonClusteredIndex",
                table: "Payroll_DeductionName",
                columns: new[] { "Name", "GLCode", "IsActive", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DepositAllowanceHistory_NonClusteredIndex",
                table: "Payroll_DepositAllowanceHistory",
                columns: new[] { "EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_DepositAllowancePaymentHistory_NonClusteredIndex",
                table: "Payroll_DepositAllowancePaymentHistory",
                columns: new[] { "EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeBonusTaxProcess_NonClusteredIndex",
                table: "Payroll_EmployeeBonusTaxProcess",
                columns: new[] { "EmployeeId", "BonusMonth", "BonusYear", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeBonusTaxProcessDetail_NonClusteredIndex",
                table: "Payroll_EmployeeBonusTaxProcessDetail",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeBonusTaxProcessSlab_NonClusteredIndex",
                table: "Payroll_EmployeeBonusTaxProcessSlab",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeDepositAllowanceConfig_NonClusteredIndex",
                table: "Payroll_EmployeeDepositAllowanceConfig",
                columns: new[] { "EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeExcludedFromBonus_NonClusteredIndex",
                table: "Payroll_EmployeeExcludedFromBonus",
                columns: new[] { "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeFreeCar_NonClusteredIndex",
                table: "Payroll_EmployeeFreeCar",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeProjectedAllowance_NonClusteredIndex",
                table: "Payroll_EmployeeProjectedAllowance",
                columns: new[] { "EmployeeId", "FiscalYearId", "PaymentMonth", "PaymentYear", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeProjectedAllowance_ProjectedAllowanceProcessInfoId",
                table: "Payroll_EmployeeProjectedAllowance",
                column: "ProjectedAllowanceProcessInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeProjectedAllowanceProcessInfo_NonClusteredIndex",
                table: "Payroll_EmployeeProjectedAllowanceProcessInfo",
                columns: new[] { "FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeTaxProcess_NonClusteredIndex",
                table: "Payroll_EmployeeTaxProcess",
                columns: new[] { "EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeTaxProcessDetail_NonClusteredIndex",
                table: "Payroll_EmployeeTaxProcessDetail",
                columns: new[] { "EmployeeId", "AllowanceNameId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeTaxProcessSlab_NonClusteredIndex",
                table: "Payroll_EmployeeTaxProcessSlab",
                columns: new[] { "EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeTaxReturnSubmission_NonClusteredIndex",
                table: "Payroll_EmployeeTaxReturnSubmission",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeTaxZone_NonClusteredIndex",
                table: "Payroll_EmployeeTaxZone",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_EmployeeYearlyInvestment_NonClusteredIndex",
                table: "Payroll_EmployeeYearlyInvestment",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_FiscalYear_NonClusteredIndex",
                table: "Payroll_FiscalYear",
                columns: new[] { "FiscalYearRange", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_IncomeTaxSetting_NonClusteredIndex",
                table: "Payroll_IncomeTaxSetting",
                columns: new[] { "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_IncomeTaxSlab_NonClusteredIndex",
                table: "Payroll_IncomeTaxSlab",
                columns: new[] { "FiscalYearId", "ImpliedCondition", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAlllowanceConfig_NonClusteredIndex",
                table: "Payroll_MonthlyAlllowanceConfig",
                columns: new[] { "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyAllowanceConfigParameter_MonthlyAlllowanceConfigId",
                table: "Payroll_MonthlyAllowanceConfigParameter",
                column: "MonthlyAlllowanceConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyIncentiveProcessDetail_MonthlyIncentiveProcessId",
                table: "Payroll_MonthlyIncentiveProcessDetail",
                column: "MonthlyIncentiveProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyVariableAllowance_AllowanceNameId",
                table: "Payroll_MonthlyVariableAllowance",
                column: "AllowanceNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyVariableAllowance_NonClusteredIndex",
                table: "Payroll_MonthlyVariableAllowance",
                columns: new[] { "EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyVariableDeduction_DeductionNameId",
                table: "Payroll_MonthlyVariableDeduction",
                column: "DeductionNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_MonthlyVariableDeduction_NonClusteredIndex",
                table: "Payroll_MonthlyVariableDeduction",
                columns: new[] { "EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_AllowanceHead_NonClusteredIndex",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                columns: new[] { "AllownanceNameId", "SpecifyFor", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_PeriodicallyVariableAllowanceDetail_PeriodicallyVariableAllowanceInfoId",
                table: "Payroll_PeriodicallyVariableAllowanceDetail",
                column: "PeriodicallyVariableAllowanceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_AllowanceHeadPayroll_PeriodicallyVariableAllowanceInfo_NonClusteredIndex",
                table: "Payroll_PeriodicallyVariableAllowanceInfo",
                columns: new[] { "SpecifyFor", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_PeriodicallyVariableDeductionDetail_PeriodicallyVariableDeductionInfoId",
                table: "Payroll_PeriodicallyVariableDeductionDetail",
                column: "PeriodicallyVariableDeductionInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_PeriodicallyVariableDeductionInfo_DeductionNameId",
                table: "Payroll_PeriodicallyVariableDeductionInfo",
                column: "DeductionNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_PeriodicallyVariableDeductionInfoeHead_NonClusteredIndex",
                table: "Payroll_PeriodicallyVariableDeductionInfo",
                columns: new[] { "SalaryVariableFor", "AmountBaseOn", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_QuarterlyIncentiveProcessDetail_QuarterlyIncentiveProcessId",
                table: "Payroll_QuarterlyIncentiveProcessDetail",
                column: "QuarterlyIncentiveProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_RecipientsofServiceAnniversaryAllowance_NonClusteredIndex",
                table: "Payroll_RecipientsofServiceAnniversaryAllowance",
                columns: new[] { "EmployeeId", "AllowanceNameId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryAllowance_NonClusteredIndex",
                table: "Payroll_SalaryAllowance",
                columns: new[] { "EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryAllowanceAdjustment_NonClusteredIndex",
                table: "Payroll_SalaryAllowanceAdjustment",
                columns: new[] { "EmployeeId", "AllowanceNameId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryAllowanceArrear_NonClusteredIndex",
                table: "Payroll_SalaryAllowanceArrear",
                columns: new[] { "EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryAllowanceArrearAdjustment_AllowanceNameId",
                table: "Payroll_SalaryAllowanceArrearAdjustment",
                column: "AllowanceNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryAllowanceConfigurationDetails_SalaryAllowanceConfigId",
                table: "Payroll_SalaryAllowanceConfigurationDetails",
                column: "SalaryAllowanceConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryAllowanceConfigurationInfo_NonClusteredIndex",
                table: "Payroll_SalaryAllowanceConfigurationInfo",
                columns: new[] { "ConfigCategory", "BaseType", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryDeduction_NonClusteredIndex",
                table: "Payroll_SalaryDeduction",
                columns: new[] { "EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryDeductionAdjustment_NonClusteredIndex",
                table: "Payroll_SalaryDeductionAdjustment",
                columns: new[] { "DeductionNameId", "EmployeeId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryHold_NonClusteredIndex",
                table: "Payroll_SalaryHold",
                columns: new[] { "EmployeeId", "IsHolded", "HoldFrom", "HoldTo", "StateStatus", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryProcess_NonClusteredIndex",
                table: "Payroll_SalaryProcess",
                columns: new[] { "ProcessType", "BatchNo", "SalaryMonth", "SalaryYear", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryProcessDetail_NonClusteredIndex",
                table: "Payroll_SalaryProcessDetail",
                columns: new[] { "EmployeeId", "FiscalYearId", "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryProcessDetail_SalaryProcessId",
                table: "Payroll_SalaryProcessDetail",
                column: "SalaryProcessId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryProcessSummery_NonClusteredIndex",
                table: "Payroll_SalaryProcessSummery",
                columns: new[] { "SalaryMonth", "SalaryYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryReviewDetail_NonClusteredIndex",
                table: "Payroll_SalaryReviewDetail",
                columns: new[] { "EmployeeId", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryReviewDetail_SalaryReviewInfoId",
                table: "Payroll_SalaryReviewDetail",
                column: "SalaryReviewInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SalaryReviewInfo_NonClusteredIndex",
                table: "Payroll_SalaryReviewInfo",
                columns: new[] { "EmployeeId", "BaseType", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_ServiceAnniversaryAllowance_NonClusteredIndex",
                table: "Payroll_ServiceAnniversaryAllowance",
                columns: new[] { "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SupplementaryPaymentAmount_NonClusteredIndex",
                table: "Payroll_SupplementaryPaymentAmount",
                columns: new[] { "EmployeeId", "PaymentMonth", "PaymentYear", "AllowanceNameId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SupplementaryPaymentProcessInfo_NonClusteredIndex",
                table: "Payroll_SupplementaryPaymentProcessInfo",
                columns: new[] { "FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SupplementaryPaymentTaxDetail_NonClusteredIndex",
                table: "Payroll_SupplementaryPaymentTaxDetail",
                columns: new[] { "EmployeeId", "AllowanceNameId", "FiscalYearId", "PaymentMonth", "PaymentYear", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SupplementaryPaymentTaxInfo_NonClusteredIndex",
                table: "Payroll_SupplementaryPaymentTaxInfo",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_SupplementaryPaymentTaxSlab_NonClusteredIndex",
                table: "Payroll_SupplementaryPaymentTaxSlab",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_TaxChallan_NonClusteredIndex",
                table: "Payroll_TaxChallan",
                columns: new[] { "EmployeeId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_TaxDocumentSubmission_NonClusteredIndex",
                table: "Payroll_TaxDocumentSubmission",
                columns: new[] { "EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_TaxExemptionSetting_NonClusteredIndex",
                table: "Payroll_TaxExemptionSetting",
                columns: new[] { "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_TaxInvestmentSetting_NonClusteredIndex",
                table: "Payroll_TaxInvestmentSetting",
                columns: new[] { "FiscalYearId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_UploadAllowance_NonClusteredIndex",
                table: "Payroll_UploadAllowance",
                columns: new[] { "EmployeeId", "AllowanceNameId", "Month", "Year", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_UploadCashSalary_CashSalaryHeadId",
                table: "Payroll_UploadCashSalary",
                column: "CashSalaryHeadId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_UploadDeduction_NonClusteredIndex",
                table: "Payroll_UploadDeduction",
                columns: new[] { "EmployeeId", "Month", "Year", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_WalletPaymentConfiguration_InternalDesignationId",
                table: "Payroll_WalletPaymentConfiguration",
                column: "InternalDesignationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_AnonymousEmployeeInfo");

            migrationBuilder.DropTable(
                name: "Payroll_ActualTaxDeduction");

            migrationBuilder.DropTable(
                name: "Payroll_AllowanceConfiguration");

            migrationBuilder.DropTable(
                name: "Payroll_BonusProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_CashSalaryProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_ConditionalDepositAllowanceConfig");

            migrationBuilder.DropTable(
                name: "Payroll_ConditionalProjectedPaymentDetail");

            migrationBuilder.DropTable(
                name: "Payroll_ConditionalProjectedPaymentExcludeParameter");

            migrationBuilder.DropTable(
                name: "Payroll_ConditionalProjectedPaymentParameter");

            migrationBuilder.DropTable(
                name: "Payroll_DeductionConfiguration");

            migrationBuilder.DropTable(
                name: "Payroll_DepositAllowanceHistory");

            migrationBuilder.DropTable(
                name: "Payroll_DepositAllowancePaymentHistory");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeBonusTaxProcess");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeBonusTaxProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeBonusTaxProcessSlab");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeDepositAllowanceConfig");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeExcludedFromBonus");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeFreeCar");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeProjectedAllowance");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeTaxProcess");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeTaxProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeTaxProcessSlab");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeTaxReturnSubmission");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeTaxZone");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeYearlyInvestment");

            migrationBuilder.DropTable(
                name: "Payroll_FiscalYear");

            migrationBuilder.DropTable(
                name: "Payroll_IncomeTaxSetting");

            migrationBuilder.DropTable(
                name: "Payroll_IncomeTaxSlab");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyAllowanceConfigParameter");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyIncentiveProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyVariableAllowance");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyVariableDeduction");

            migrationBuilder.DropTable(
                name: "Payroll_PeriodicallyVariableAllowanceDetail");

            migrationBuilder.DropTable(
                name: "Payroll_PeriodicallyVariableDeductionDetail");

            migrationBuilder.DropTable(
                name: "Payroll_PrincipleAmountInfo");

            migrationBuilder.DropTable(
                name: "Payroll_QuarterlyIncentiveProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_RecipientsofServiceAnniversaryAllowance");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryAllowance");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryAllowanceAdjustment");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryAllowanceArrear");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryAllowanceArrearAdjustment");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryAllowanceConfigurationDetails");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryDeduction");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryDeductionAdjustment");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryHold");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryProcessDetail");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryProcessSummery");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryReviewDetail");

            migrationBuilder.DropTable(
                name: "Payroll_ServiceAnniversaryAllowance");

            migrationBuilder.DropTable(
                name: "Payroll_SupplementaryPaymentAmount");

            migrationBuilder.DropTable(
                name: "Payroll_SupplementaryPaymentProcessInfo");

            migrationBuilder.DropTable(
                name: "Payroll_SupplementaryPaymentTaxDetail");

            migrationBuilder.DropTable(
                name: "Payroll_SupplementaryPaymentTaxInfo");

            migrationBuilder.DropTable(
                name: "Payroll_SupplementaryPaymentTaxSlab");

            migrationBuilder.DropTable(
                name: "Payroll_TaxChallan");

            migrationBuilder.DropTable(
                name: "Payroll_TaxDocumentSubmission");

            migrationBuilder.DropTable(
                name: "Payroll_TaxExemptionSetting");

            migrationBuilder.DropTable(
                name: "Payroll_TaxInvestmentSetting");

            migrationBuilder.DropTable(
                name: "Payroll_UploadAllowance");

            migrationBuilder.DropTable(
                name: "Payroll_UploadCashSalary");

            migrationBuilder.DropTable(
                name: "Payroll_UploadDeduction");

            migrationBuilder.DropTable(
                name: "Payroll_WalletPaymentConfiguration");

            migrationBuilder.DropTable(
                name: "Payroll_BonusProcess");

            migrationBuilder.DropTable(
                name: "Payroll_CashSalaryProcess");

            migrationBuilder.DropTable(
                name: "Payroll_ConditionalProjectedPayment");

            migrationBuilder.DropTable(
                name: "Payroll_EmployeeProjectedAllowanceProcessInfo");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyAlllowanceConfig");

            migrationBuilder.DropTable(
                name: "Payroll_MonthlyIncentiveProcess");

            migrationBuilder.DropTable(
                name: "Payroll_PeriodicallyVariableAllowanceInfo");

            migrationBuilder.DropTable(
                name: "Payroll_PeriodicallyVariableDeductionInfo");

            migrationBuilder.DropTable(
                name: "Payroll_QuarterlyIncentiveProcess");

            migrationBuilder.DropTable(
                name: "Payroll_AllowanceName");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryAllowanceConfigurationInfo");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryProcess");

            migrationBuilder.DropTable(
                name: "Payroll_SalaryReviewInfo");

            migrationBuilder.DropTable(
                name: "Payroll_CashSalaryHead");

            migrationBuilder.DropTable(
                name: "HR_InternalDesignations");

            migrationBuilder.DropTable(
                name: "Payroll_BonusConfig");

            migrationBuilder.DropTable(
                name: "Payroll_DeductionName");

            migrationBuilder.DropTable(
                name: "Payroll_AllowanceHead");

            migrationBuilder.DropTable(
                name: "Payroll_Bonus");

            migrationBuilder.DropTable(
                name: "Payroll_DeductionHead");
        }
    }
}
