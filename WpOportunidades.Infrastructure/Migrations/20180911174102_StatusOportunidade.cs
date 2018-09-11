using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WpOportunidades.Infrastructure.Migrations
{
    public partial class StatusOportunidade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserXOportunidades");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserXOportunidades",
                newName: "ID");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    UserXOportunidadeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Status_UserXOportunidades_UserXOportunidadeId",
                        column: x => x.UserXOportunidadeId,
                        principalTable: "UserXOportunidades",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Status_UserXOportunidadeId",
                table: "Status",
                column: "UserXOportunidadeId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UserXOportunidades",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "UserXOportunidades",
                nullable: false,
                defaultValue: "");
        }
    }
}
