using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Home.Db.Migrations
{
    /// <inheritdoc />
    public partial class StateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "allow_switch_state",
                table: "device",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "power_on_behavior",
                table: "device",
                type: "TEXT",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_switch_state",
                table: "device");

            migrationBuilder.DropColumn(
                name: "power_on_behavior",
                table: "device");
        }
    }
}
