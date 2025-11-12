using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServicoOportunidades.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Fichas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    RepresentanteId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatusFicha = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TituloObra = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    DescricaoSimples = table.Column<string>(type: "TEXT", nullable: true),
                    ValorEstimado = table.Column<double>(type: "REAL", nullable: true),
                    AreaM2 = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fichas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fichas");
        }
    }
}
