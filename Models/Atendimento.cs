using API_sis_conselhotutelarv2.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_sis_conselhotutelarv2.Models
{
    public class Atendimento
    {
        public int Ate_Id { get; set; }
        public string Ate_Codigo { get; set; }
        public DateTime Ate_Data { get; set; }
        public StatusAtendimento Ate_Status { get; set; }
        public string Ate_Descritivo { get; set; }
        public int? Ate_IdCliente { get; set; }
        public int Ate_IdColaborador { get; set; }
        [NotMapped]
        public string? NomeCidadao { get; set; }
        [NotMapped]
        public string NomeAtendente { get; set; }
        public Cliente Cidadao { get; set; }
        public Colaborador Colaborador { get; set; }
    }
}
