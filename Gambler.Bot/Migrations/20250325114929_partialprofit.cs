using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gambler.Bot.Strategies.Migrations
{
    /// <inheritdoc />
    public partial class partialprofit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PartialProfit",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalProfit",
                table: "Sessions",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PartialProfit",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "TotalProfit",
                table: "Sessions");
        }
    }
}
