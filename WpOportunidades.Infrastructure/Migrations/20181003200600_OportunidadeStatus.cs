using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WpOportunidades.Infrastructure.Migrations
{
    public partial class OportunidadeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserXOportunidades_Status_StatusID",
                table: "UserXOportunidades");

            migrationBuilder.AlterColumn<int>(
                name: "StatusID",
                table: "UserXOportunidades",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "Oportunidades",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OportunidadeStatusID",
                table: "Oportunidades",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OportunidadeStatuses",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OportunidadeStatuses", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Oportunidades_OportunidadeStatusID",
                table: "Oportunidades",
                column: "OportunidadeStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_Oportunidades_OportunidadeStatuses_OportunidadeStatusID",
                table: "Oportunidades",
                column: "OportunidadeStatusID",
                principalTable: "OportunidadeStatuses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserXOportunidades_Status_StatusID",
                table: "UserXOportunidades",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Oportunidades_OportunidadeStatuses_OportunidadeStatusID",
                table: "Oportunidades");

            migrationBuilder.DropForeignKey(
                name: "FK_UserXOportunidades_Status_StatusID",
                table: "UserXOportunidades");

            migrationBuilder.DropTable(
                name: "OportunidadeStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Oportunidades_OportunidadeStatusID",
                table: "Oportunidades");

            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "Oportunidades");

            migrationBuilder.DropColumn(
                name: "OportunidadeStatusID",
                table: "Oportunidades");

            migrationBuilder.AlterColumn<int>(
                name: "StatusID",
                table: "UserXOportunidades",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_UserXOportunidades_Status_StatusID",
                table: "UserXOportunidades",
                column: "StatusID",
                principalTable: "Status",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
