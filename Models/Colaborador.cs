using System.ComponentModel.DataAnnotations;

namespace API_sis_conselhotutelarv2.Models
{
    public class Colaborador
    {
        public int Col_Id { get; set; }
        public string Col_Nome { get; set; }
        public string Col_Username { get; set; }
        [EmailAddress]
        public string Col_Email { get; set; }
        public string? Col_Telefone { get; set; }
        public string Col_Senha { get; set; }
        public int Col_IdCargo { get; set; }
        public Cargo Cargo { get; set; }

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
