using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProjetTestOne.Data.Migrations
{
    public partial class DeleteStatusFromWork : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Works");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateUserDto");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Works",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
