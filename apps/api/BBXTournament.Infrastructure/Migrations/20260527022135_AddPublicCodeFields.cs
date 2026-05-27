using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BBXTournament.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPublicCodeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PublicCode",
                table: "Tournaments",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicCode",
                table: "Communities",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_PublicCode",
                table: "Tournaments",
                column: "PublicCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communities_PublicCode",
                table: "Communities",
                column: "PublicCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tournaments_PublicCode",
                table: "Tournaments");

            migrationBuilder.DropIndex(
                name: "IX_Communities_PublicCode",
                table: "Communities");

            migrationBuilder.DropColumn(
                name: "PublicCode",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "PublicCode",
                table: "Communities");
        }
    }
}
