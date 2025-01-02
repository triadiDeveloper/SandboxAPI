using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Organization");

            migrationBuilder.CreateTable(
                name: "Company",
                schema: "Organization",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Note = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    CreatedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CAST(sysdatetimeoffset() AT TIME ZONE 'SE Asia Standard Time' AS DATETIME)"),
                    ModifiedUser = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Version = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Code",
                schema: "Organization",
                table: "Company",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_Name",
                schema: "Organization",
                table: "Company",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company",
                schema: "Organization");
        }
    }
}
