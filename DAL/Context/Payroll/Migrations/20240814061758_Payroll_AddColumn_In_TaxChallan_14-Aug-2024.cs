using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Payroll.Migrations
{
    /// <inheritdoc />
    public partial class Payroll_AddColumn_In_TaxChallan_14Aug2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Month",
                table: "Payroll_TaxChallan",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TaxDate",
                table: "Payroll_TaxChallan",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Month",
                table: "Payroll_TaxChallan");

            migrationBuilder.DropColumn(
                name: "TaxDate",
                table: "Payroll_TaxChallan");
        }
    }
}
