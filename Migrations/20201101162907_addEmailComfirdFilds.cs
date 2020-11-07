using Microsoft.EntityFrameworkCore.Migrations;

namespace PracaDyplomowa.Migrations
{
    public partial class addEmailComfirdFilds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Comfirmed",
                table: "FirmAccounts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ConfirmatioCode",
                table: "FirmAccounts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comfirmed",
                table: "FirmAccounts");

            migrationBuilder.DropColumn(
                name: "ConfirmatioCode",
                table: "FirmAccounts");
        }
    }
}
