using System.ComponentModel.DataAnnotations;

namespace API_sis_conselhotutelarv2.Models
{
    public class ColaboradorEdicaoDto
    {
        public int Col_Id { get; set; }
        public string? Col_Nome { get; set; }
        public string? Col_Username { get; set; }
        [EmailAddress]
        public string? Col_Email { get; set; }
        public string? Col_Telefone { get; set; }
        public int? Col_IdCargo { get; set; }
        public int? Col_IdEmpresa { get; set; }
    }
}
