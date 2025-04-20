using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gambler.Bot.Strategies.Migrations
{
    /// <inheritdoc />
    public partial class something : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BettingSpeed",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BettingSpeed",
                table: "Sessions");
        }
    }
}
