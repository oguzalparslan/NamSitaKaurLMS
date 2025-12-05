using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamSitaKaurLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addIsPublishedFieldWithCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublished",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublished",
                table: "Courses");
        }
    }
}
