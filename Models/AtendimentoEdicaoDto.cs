using API_sis_conselhotutelarv2.Enums;

namespace API_sis_conselhotutelarv2.Models
{
    public class AtendimentoEdicaoDto
    {
        public int Ate_Id { get; set; }
        public string? Ate_Codigo { get; set; }
        public DateTime? Ate_Data { get; set; }
        public StatusAtendimento? Ate_Status { get; set; }
        public string? Ate_Descritivo { get; set; }
        public int? Ate_IdCliente { get; set; }
        public int? Ate_IdColaborador { get; set; }
    }
}
