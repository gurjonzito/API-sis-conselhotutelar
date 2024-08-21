namespace API_sis_conselhotutelarv2.Models
{
    public class ClienteDto
    {
        private DateTime dataNasc;

        public int Cli_Id { get; set; }
        public string Cli_Nome { get; set; }
        public string Cli_CPF { get; set; }
        public DateTime Cli_DataNasc
        {
            get => dataNasc;
            set => dataNasc = value.Date; // Garante que a hora esteja sempre zerada
        }
        public string? Cli_Email { get; set; }
        public string Cli_Telefone { get; set; }
        public int? Cli_IdFamilia { get; set; }
        public int Ativo_Inativo { get; set; } = 1;
    }
}
