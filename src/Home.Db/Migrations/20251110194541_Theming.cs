using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home.Db.Migrations
{
    /// <inheritdoc />
    public partial class Theming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "theme",
                columns: table => new
                {
                    id = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false, collation: "NOCASE"),
                    primary = table.Column<string>(type: "TEXT", nullable: false),
                    secondary = table.Column<string>(type: "TEXT", nullable: false),
                    tertiary = table.Column<string>(type: "TEXT", nullable: false),
                    background = table.Column<byte[]>(type: "BLOB", nullable: false),
                    created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    last_updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theme", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_theme_name",
                table: "theme",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "theme");
        }
    }
}
