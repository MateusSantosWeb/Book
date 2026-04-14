using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookShelfAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixMetaLeituraLivroPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MetaLeituraLivros",
                table: "MetaLeituraLivros");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MetaLeituraLivros",
                table: "MetaLeituraLivros",
                columns: new[] { "MetaLeituraId", "LivroId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MetaLeituraLivros",
                table: "MetaLeituraLivros");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MetaLeituraLivros",
                table: "MetaLeituraLivros",
                column: "MetaLeituraId");
        }
    }
}
