using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHomeFinalProject.Migrations
{
    public partial class addButtonlink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ButtonLink",
                table: "SliderImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ButtonLink",
                table: "SliderImages");
        }
    }
}
