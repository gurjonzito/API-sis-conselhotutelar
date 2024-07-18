using System.ComponentModel.DataAnnotations;

namespace API_sis_conselhotutelarv2.Models
{
    public class Cliente
    {
        public int Cli_Id { get; set; }
        public string Cli_Nome { get; set; }
        public DateOnly Cli_DataNasc { get; set; }
        public string? Cli_Email { get; set; }
        public string? Cli_Telefone { get; set; }
        public int? Cli_IdFamilia { get; set; }
        public Familia Familia { get; set; }
    }
}
