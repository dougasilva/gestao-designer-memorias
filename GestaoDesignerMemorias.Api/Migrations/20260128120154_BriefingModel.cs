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
                table: "BriefingItens",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Tipo",
                table: "BriefingItens",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Opcoes",
                table: "BriefingItens");

            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "BriefingItens");
        }
    }
}
