using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opinion_on_Quotes.Data.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dramas",
                columns: table => new
                {
                    drama_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    release_year = table.Column<int>(type: "int", nullable: false),
                    genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    synopsis = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dramas", x => x.drama_id);
                });

            migrationBuilder.CreateTable(
                name: "Moods",
                columns: table => new
                {
                    mood_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Moods", x => x.mood_id);
                });

            migrationBuilder.CreateTable(
                name: "Quotes",
                columns: table => new
                {
                    quote_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    drama_id = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    actor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    episode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotes", x => x.quote_id);
                    table.ForeignKey(
                        name: "FK_Quotes_Dramas_drama_id",
                        column: x => x.drama_id,
                        principalTable: "Dramas",
                        principalColumn: "drama_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    quote_id1 = table.Column<int>(type: "int", nullable: false),
                    quote_id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Quotes_quote_id1",
                        column: x => x.quote_id1,
                        principalTable: "Quotes",
                        principalColumn: "quote_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quote_mood",
                columns: table => new
                {
                    quote_id = table.Column<int>(type: "int", nullable: false),
                    mood_id = table.Column<int>(type: "int", nullable: false),
                    quotemood_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quote_mood", x => new { x.quote_id, x.mood_id });
                    table.ForeignKey(
                        name: "FK_quote_mood_Moods_mood_id",
                        column: x => x.mood_id,
                        principalTable: "Moods",
                        principalColumn: "mood_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_quote_mood_Quotes_quote_id",
                        column: x => x.quote_id,
                        principalTable: "Quotes",
                        principalColumn: "quote_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_quote_id1",
                table: "Comments",
                column: "quote_id1");

            migrationBuilder.CreateIndex(
                name: "IX_quote_mood_mood_id",
                table: "quote_mood",
                column: "mood_id");

            migrationBuilder.CreateIndex(
                name: "IX_Quotes_drama_id",
                table: "Quotes",
                column: "drama_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "quote_mood");

            migrationBuilder.DropTable(
                name: "Moods");

            migrationBuilder.DropTable(
                name: "Quotes");

            migrationBuilder.DropTable(
                name: "Dramas");
        }
    }
}
