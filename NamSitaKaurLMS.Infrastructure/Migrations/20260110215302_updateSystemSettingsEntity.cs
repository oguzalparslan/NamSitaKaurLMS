using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamSitaKaurLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateSystemSettingsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemSetting",
                table: "SystemSetting");

            migrationBuilder.RenameTable(
                name: "SystemSetting",
                newName: "SystemSettings");

            migrationBuilder.AddColumn<string>(
                name: "SettingCode",
                table: "SystemSettings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemSettings",
                table: "SystemSettings",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SystemSettings",
                table: "SystemSettings");

            migrationBuilder.DropColumn(
                name: "SettingCode",
                table: "SystemSettings");

            migrationBuilder.RenameTable(
                name: "SystemSettings",
                newName: "SystemSetting");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SystemSetting",
                table: "SystemSetting",
                column: "Id");
        }
    }
}
