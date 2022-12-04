using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHomeFinalProject.Migrations
{
    public partial class AddedTitleToUsefulLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "FooterUsefulLinks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "FooterUsefulLinks");
        }
    }
}
