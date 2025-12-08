using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamSitaKaurLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addQuotaFieldForCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quota",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quota",
                table: "Courses");
        }
    }
}
