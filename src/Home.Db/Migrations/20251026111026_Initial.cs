using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home.Db.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "device",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    device_type = table.Column<string>(type: "TEXT", nullable: false),
                    ieee_address = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false, collation: "NOCASE"),
                    friendly_name = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, collation: "NOCASE")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_device", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_device_friendly_name",
                table: "device",
                column: "friendly_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_device_ieee_address",
                table: "device",
                column: "ieee_address",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "device");
        }
    }
}
