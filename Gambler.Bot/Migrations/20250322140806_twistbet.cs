using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gambler.Bot.Strategies.Migrations
{
    /// <inheritdoc />
    public partial class twistbet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwistBets",
                columns: table => new
                {
                    BetID = table.Column<string>(type: "TEXT", nullable: false),
                    Roll = table.Column<decimal>(type: "TEXT", nullable: false),
                    High = table.Column<bool>(type: "INTEGER", nullable: false),
                    Chance = table.Column<decimal>(type: "TEXT", nullable: false),
                    Nonce = table.Column<long>(type: "INTEGER", nullable: false),
                    ServerHash = table.Column<string>(type: "TEXT", nullable: true),
                    ServerSeed = table.Column<string>(type: "TEXT", nullable: true),
                    ClientSeed = table.Column<string>(type: "TEXT", nullable: true),
                    WinnableType = table.Column<int>(type: "INTEGER", nullable: false),
                    Game = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<decimal>(type: "TEXT", nullable: false),
                    Profit = table.Column<decimal>(type: "TEXT", nullable: false),
                    Userid = table.Column<long>(type: "INTEGER", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    Guid = table.Column<string>(type: "TEXT", nullable: false),
                    Edge = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsWin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Site = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwistBets", x => x.BetID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwistBets");
        }
    }
}
