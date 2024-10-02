using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Employee.Migrations
{
    /// <inheritdoc />
    public partial class HR_Update_ForeignKen_In_Designation_05Sep2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_Designations_HR_Grades_GradeId",
                table: "HR_Designations");

            migrationBuilder.AlterColumn<string>(
                name: "MaxLength",
                table: "HR_TableConfig",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Max",
                table: "HR_TableConfig",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Min",
                table: "HR_TableConfig",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MinLength",
                table: "HR_TableConfig",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "HR_Designations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Designations_HR_Grades_GradeId",
                table: "HR_Designations",
                column: "GradeId",
                principalTable: "HR_Grades",
                principalColumn: "GradeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HR_Designations_HR_Grades_GradeId",
                table: "HR_Designations");

            migrationBuilder.DropColumn(
                name: "Max",
                table: "HR_TableConfig");

            migrationBuilder.DropColumn(
                name: "Min",
                table: "HR_TableConfig");

            migrationBuilder.DropColumn(
                name: "MinLength",
                table: "HR_TableConfig");

            migrationBuilder.AlterColumn<string>(
                name: "MaxLength",
                table: "HR_TableConfig",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GradeId",
                table: "HR_Designations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HR_Designations_HR_Grades_GradeId",
                table: "HR_Designations",
                column: "GradeId",
                principalTable: "HR_Grades",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
