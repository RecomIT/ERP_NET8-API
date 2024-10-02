using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Expense_Reimbursement.Migrations
{
    /// <inheritdoc />
    public partial class ExpenseandReimbursementModuleInit09June2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reimburse_Conveyance",
                columns: table => new
                {
                    ConveyanceId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "date", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SpendMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Transportation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsAccount = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Conveyance", x => x.ConveyanceId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Conveyance_Details",
                columns: table => new
                {
                    ConveyanceDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConveyanceId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Mode = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Conveyance_Details", x => x.ConveyanceDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Entertainment",
                columns: table => new
                {
                    EntertainmentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "date", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Entertainments = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SpendMode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CommentsUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsAccount = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ActualFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_Reimburse_Entertainment", x => x.EntertainmentId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Entertainment_Details",
                columns: table => new
                {
                    EntertainmentDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntertainmentId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Entertainment_Details", x => x.EntertainmentDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Expat",
                columns: table => new
                {
                    ExpatId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "date", nullable: true),
                    Expats = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsAccount = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SpendMode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Expat", x => x.ExpatId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Expat_Details",
                columns: table => new
                {
                    ExpatDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpatId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Particular = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BillType = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Costs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Expat_Details", x => x.ExpatDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Purchase",
                columns: table => new
                {
                    PurchaseId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "date", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsAccount = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Purchases = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SpendMode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdvanceAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ActualFileName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileFormat = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FileSize = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
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
                    table.PrimaryKey("PK_Reimburse_Purchase", x => x.PurchaseId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Purchase_Details",
                columns: table => new
                {
                    PurchaseDetailId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseId = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    Item = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Purchase_Details", x => x.PurchaseDetailId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Training",
                columns: table => new
                {
                    TrainingId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestDate = table.Column<DateTime>(type: "date", nullable: true),
                    InstitutionName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Course = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdmissionDate = table.Column<DateTime>(type: "date", nullable: true),
                    CommentsUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsAccount = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TrainingCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Training", x => x.TrainingId);
                });

            migrationBuilder.CreateTable(
                name: "Reimburse_Travel",
                columns: table => new
                {
                    TravelId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionDate = table.Column<DateTime>(type: "date", nullable: true),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FromDate = table.Column<DateTime>(type: "date", nullable: true),
                    ToDate = table.Column<DateTime>(type: "date", nullable: true),
                    SpendMode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Purpose = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Transportation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TransportationCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AccommodationCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SubsistenceCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OtherCosts = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommentsUser = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommentsAccount = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StateStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AccountStatus = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
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
                    RejectedRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reimburse_Travel", x => x.TravelId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Conveyance_NonClusteredIndex",
                table: "Reimburse_Conveyance",
                columns: new[] { "ConveyanceId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Conveyance_Details_NonClusteredIndex",
                table: "Reimburse_Conveyance_Details",
                columns: new[] { "ConveyanceDetailId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Entertainment_NonClusteredIndex",
                table: "Reimburse_Entertainment",
                columns: new[] { "EntertainmentId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Entertainment_Details_NonClusteredIndex",
                table: "Reimburse_Entertainment_Details",
                columns: new[] { "EntertainmentDetailId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Expat_NonClusteredIndex",
                table: "Reimburse_Expat",
                columns: new[] { "ExpatId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Expat_Details_NonClusteredIndex",
                table: "Reimburse_Expat_Details",
                columns: new[] { "ExpatDetailId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Purchase_NonClusteredIndex",
                table: "Reimburse_Purchase",
                columns: new[] { "PurchaseId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Purchase_Details_NonClusteredIndex",
                table: "Reimburse_Purchase_Details",
                columns: new[] { "PurchaseDetailId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Training_NonClusteredIndex",
                table: "Reimburse_Training",
                columns: new[] { "TrainingId", "CompanyId", "OrganizationId" });

            migrationBuilder.CreateIndex(
                name: "IX_Reimburse_Travel_NonClusteredIndex",
                table: "Reimburse_Travel",
                columns: new[] { "TravelId", "CompanyId", "OrganizationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reimburse_Conveyance");

            migrationBuilder.DropTable(
                name: "Reimburse_Conveyance_Details");

            migrationBuilder.DropTable(
                name: "Reimburse_Entertainment");

            migrationBuilder.DropTable(
                name: "Reimburse_Entertainment_Details");

            migrationBuilder.DropTable(
                name: "Reimburse_Expat");

            migrationBuilder.DropTable(
                name: "Reimburse_Expat_Details");

            migrationBuilder.DropTable(
                name: "Reimburse_Purchase");

            migrationBuilder.DropTable(
                name: "Reimburse_Purchase_Details");

            migrationBuilder.DropTable(
                name: "Reimburse_Training");

            migrationBuilder.DropTable(
                name: "Reimburse_Travel");
        }
    }
}
