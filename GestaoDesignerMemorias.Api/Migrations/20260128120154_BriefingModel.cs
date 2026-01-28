using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDesignerMemorias.Api.Migrations
{
    /// <inheritdoc />
    public partial class BriefingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Opcoes",
                table: "BriefingItems",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "BriefingItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opcoes",
                table: "BriefingItems");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "BriefingItems");
        }
    }
}
