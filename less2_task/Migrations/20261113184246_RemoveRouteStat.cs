using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace less2_task.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRouteStat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RouteStats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RouteStats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Count = table.Column<int>(type: "INTEGER", nullable: false),
                    Method = table.Column<string>(type: "TEXT", nullable: false),
                    RouteName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RouteStats", x => x.Id);
                });
        }
    }
}
