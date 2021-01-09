using Microsoft.EntityFrameworkCore.Migrations;

namespace PracaDyplomowa.Migrations
{
    public partial class AddTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "Events",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "EventeTags",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventeTags", x => new { x.TagId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventeTags_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventeTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_TagId",
                table: "Events",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_EventeTags_EventId",
                table: "EventeTags",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Tags_TagId",
                table: "Events",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "TagId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Tags_TagId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventeTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Events_TagId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "Events");
        }
    }
}
