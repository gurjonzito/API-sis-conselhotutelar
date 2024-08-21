using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_sis_conselhotutelarv2.Migrations
{
    public partial class MigracaDois : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChaveValidade_Emp_RazaoSocial",
                table: "ChaveValidade");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ChaveValidade_Emp_RazaoSocial",
                table: "ChaveValidade",
                column: "Emp_RazaoSocial",
                unique: true);
        }
    }
}
