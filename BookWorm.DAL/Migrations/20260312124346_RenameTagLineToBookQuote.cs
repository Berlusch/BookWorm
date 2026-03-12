using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameTagLineToBookQuote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookQuotes_BookTitleId",
                table: "BookQuotes");

            migrationBuilder.CreateIndex(
                name: "IX_BookQuotes_BookTitleId",
                table: "BookQuotes",
                column: "BookTitleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BookQuotes_BookTitleId",
                table: "BookQuotes");

            migrationBuilder.CreateIndex(
                name: "IX_BookQuotes_BookTitleId",
                table: "BookQuotes",
                column: "BookTitleId",
                unique: true);
        }
    }
}
