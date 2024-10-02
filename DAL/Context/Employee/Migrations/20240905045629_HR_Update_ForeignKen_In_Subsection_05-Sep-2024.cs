using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Employee.Migrations
{
    /// <inheritdoc />
    public partial class HR_Update_ForeignKen_In_Subsection_05Sep2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_SubSections_HR_Sections_SectionId",
                table: "HR_SubSections");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "HR_SubSections",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_SubSections_HR_Sections_SectionId",
                table: "HR_SubSections",
                column: "SectionId",
                principalTable: "HR_Sections",
                principalColumn: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_SubSections_HR_Sections_SectionId",
                table: "HR_SubSections");

            migrationBuilder.AlterColumn<int>(
                name: "SectionId",
                table: "HR_SubSections",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_SubSections_HR_Sections_SectionId",
                table: "HR_SubSections",
                column: "SectionId",
                principalTable: "HR_Sections",
                principalColumn: "SectionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
