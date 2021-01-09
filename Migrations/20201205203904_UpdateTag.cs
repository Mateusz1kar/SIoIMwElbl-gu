using Microsoft.EntityFrameworkCore.Migrations;

namespace PracaDyplomowa.Migrations
{
    public partial class UpdateTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nazwa",
                table: "Tags");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tags",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tags");

            migrationBuilder.AddColumn<string>(
                name: "Nazwa",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
