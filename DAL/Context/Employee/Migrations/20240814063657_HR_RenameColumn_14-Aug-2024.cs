using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Employee.Migrations
{
    /// <inheritdoc />
    public partial class HR_RenameColumn_14Aug2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Screen",
                table: "HR_TableConfig",
                newName: "Purpose");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Purpose",
                table: "HR_TableConfig",
                newName: "Screen");
        }
    }
}
