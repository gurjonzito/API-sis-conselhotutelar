using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_sis_conselhotutelarv2.Migrations
{
    public partial class MigracaoInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChaveValidade",
                columns: table => new
                {
                    Emp_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Emp_RazaoSocial = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Emp_CNPJ = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Emp_Telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativo_Inativo = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Emp_Connection = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChaveValidade", x => x.Emp_Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChavesValidade",
                columns: table => new
                {
                    Cha_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cha_Chave = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cha_Validade = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Cha_IdEmpresa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChavesValidade", x => x.Cha_Id);
                    table.ForeignKey(
                        name: "FK_ChavesValidade_ChaveValidade_Cha_IdEmpresa",
                        column: x => x.Cha_IdEmpresa,
                        principalTable: "ChaveValidade",
                        principalColumn: "Emp_Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ChavesValidade_Cha_Chave",
                table: "ChavesValidade",
                column: "Cha_Chave",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChavesValidade_Cha_IdEmpresa",
                table: "ChavesValidade",
                column: "Cha_IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_ChaveValidade_Emp_CNPJ",
                table: "ChaveValidade",
                column: "Emp_CNPJ",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChaveValidade_Emp_RazaoSocial",
                table: "ChaveValidade",
                column: "Emp_RazaoSocial",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChavesValidade");

            migrationBuilder.DropTable(
                name: "ChaveValidade");
        }
    }
}
