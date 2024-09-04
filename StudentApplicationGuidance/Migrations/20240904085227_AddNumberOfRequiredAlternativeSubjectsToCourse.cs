using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentApplicationGuidance.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfRequiredAlternativeSubjectsToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRequiredAlternativeSubjects",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRequiredAlternativeSubjects",
                table: "Courses");
        }
    }
}
