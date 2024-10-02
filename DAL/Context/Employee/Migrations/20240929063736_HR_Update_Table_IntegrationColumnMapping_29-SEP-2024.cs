using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Employee.Migrations
{
    /// <inheritdoc />
    public partial class HR_Update_Table_IntegrationColumnMapping_29SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Passphrase",
                table: "HR_IntegrationConfig",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passphrase",
                table: "HR_IntegrationConfig");
        }
    }
}
