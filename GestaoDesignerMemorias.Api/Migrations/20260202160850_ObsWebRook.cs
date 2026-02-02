using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDesignerMemorias.Api.Migrations
{
    /// <inheritdoc />
    public partial class ObsWebRook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Observacao",
                table: "MensagensWebhook",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Observacao",
                table: "MensagensWebhook");
        }
    }
}
