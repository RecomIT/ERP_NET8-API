using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddBaseColumn_In_Arrear_Adjustment_26SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelRemarks",
                table: "Payroll_SalaryAllowanceArrearAdjustment",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CancelledBy",
                table: "Payroll_SalaryAllowanceArrearAdjustment",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "Payroll_SalaryAllowanceArrearAdjustment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedBy",
                table: "Payroll_SalaryAllowanceArrearAdjustment",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedDate",
                table: "Payroll_SalaryAllowanceArrearAdjustment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectedRemarks",
                table: "Payroll_SalaryAllowanceArrearAdjustment",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelRemarks",
                table: "Payroll_SalaryAllowanceArrearAdjustment");

            migrationBuilder.DropColumn(
                name: "CancelledBy",
                table: "Payroll_SalaryAllowanceArrearAdjustment");

            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "Payroll_SalaryAllowanceArrearAdjustment");

            migrationBuilder.DropColumn(
                name: "RejectedBy",
                table: "Payroll_SalaryAllowanceArrearAdjustment");

            migrationBuilder.DropColumn(
                name: "RejectedDate",
                table: "Payroll_SalaryAllowanceArrearAdjustment");

            migrationBuilder.DropColumn(
                name: "RejectedRemarks",
                table: "Payroll_SalaryAllowanceArrearAdjustment");
        }
    }
}
