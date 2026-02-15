using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamSitaKaurLMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addLessonDateForLessons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LessonDate",
                table: "Lessons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LessonDate",
                table: "Lessons");
        }
    }
}
