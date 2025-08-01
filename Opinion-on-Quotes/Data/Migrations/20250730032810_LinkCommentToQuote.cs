using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Opinion_on_Quotes.Data.Migrations
{
    /// <inheritdoc />
    public partial class LinkCommentToQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Quotes_quote_id1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_quote_id1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "quote_id1",
                table: "Comments");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_quote_id",
                table: "Comments",
                column: "quote_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Quotes_quote_id",
                table: "Comments",
                column: "quote_id",
                principalTable: "Quotes",
                principalColumn: "quote_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Quotes_quote_id",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_Comments_quote_id",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "quote_id1",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Comments_quote_id1",
                table: "Comments",
                column: "quote_id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Quotes_quote_id1",
                table: "Comments",
                column: "quote_id1",
                principalTable: "Quotes",
                principalColumn: "quote_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
