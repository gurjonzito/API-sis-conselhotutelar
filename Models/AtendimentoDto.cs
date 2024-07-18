using API_sis_conselhotutelarv2.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_sis_conselhotutelarv2.Models
{
    public class AtendimentoDto
    {
        public string Ate_Codigo { get; set; }
        public DateTime Ate_Data { get; set; }
        public string Ate_Status { get; set; }
        public string Ate_Descritivo { get; set; }
        public int? Ate_IdCliente { get; set; }
        public int Ate_IdColaborador { get; set; }
    }
}
