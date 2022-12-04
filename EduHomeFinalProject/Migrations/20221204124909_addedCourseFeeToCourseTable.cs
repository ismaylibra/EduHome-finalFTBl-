using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHomeFinalProject.Migrations
{
    public partial class addedCourseFeeToCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CourseFee",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseFee",
                table: "Courses");
        }
    }
}
