using System.ComponentModel.DataAnnotations;

namespace API_sis_conselhotutelarv2.Models
{
    public class ColaboradorDto
    {
        public int Col_Id { get; set; }
        public string Col_Nome { get; set; }
        public string Col_Username { get; set; }
        [EmailAddress]
        public string Col_Email { get; set; }
        public string? Col_Telefone { get; set; }
        public string Col_Senha { get; set; }
        public int Col_IdCargo { get; set; }
        public int Col_IdEmpresa { get; set; }
        public int Ativo_Inativo { get; set; } = 1;

        public void SetSenha(string senha)
        {
            Col_Senha = BCrypt.Net.BCrypt.HashPassword(senha);
        }

        public bool VerificarSenha(string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, Col_Senha);
        }
    }
}
