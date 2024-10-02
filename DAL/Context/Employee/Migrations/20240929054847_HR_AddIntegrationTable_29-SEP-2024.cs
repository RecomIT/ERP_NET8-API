using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Context.Employee.Migrations
{
    /// <inheritdoc />
    public partial class HR_AddIntegrationTable_29SEP2024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HR_IntegrationConfig",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    GateWay = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DataType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Host = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Port = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DataTransferType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AuthenticationPublicKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AuthenticationPrivateKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    AuthenticationPrivateKeyFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CertificateFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EncryptionPublicKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EncryptionPrivateKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    EncryptionPrivateKeyFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecryptionPublicKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DecryptionPrivateKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DecryptionPrivateKeyFilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_IntegrationConfig", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR_IntegrationModule",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Module = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IntegrationConfigId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_IntegrationModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_IntegrationModule_HR_IntegrationConfig_IntegrationConfigId",
                        column: x => x.IntegrationConfigId,
                        principalTable: "HR_IntegrationConfig",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HR_IntegrationColumnMapping",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceColumn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SystemColumn = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    SystemTable = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    IntegrationModuleId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrganizationId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR_IntegrationColumnMapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HR_IntegrationColumnMapping_HR_IntegrationModule_IntegrationModuleId",
                        column: x => x.IntegrationModuleId,
                        principalTable: "HR_IntegrationModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HR_IntegrationColumnMapping_IntegrationModuleId",
                table: "HR_IntegrationColumnMapping",
                column: "IntegrationModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_HR_IntegrationModule_IntegrationConfigId",
                table: "HR_IntegrationModule",
                column: "IntegrationConfigId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HR_IntegrationColumnMapping");

            migrationBuilder.DropTable(
                name: "HR_IntegrationModule");

            migrationBuilder.DropTable(
                name: "HR_IntegrationConfig");
        }
    }
}
