using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EduHomeFinalProject.Migrations
{
    public partial class AddedFooterInformationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FooterInformations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admission = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcademicCalendar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventList = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HostelAndDinning = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeTable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FooterInformations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FooterInformations");
        }
    }
}
