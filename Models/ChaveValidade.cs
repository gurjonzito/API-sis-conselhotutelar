namespace API_sis_conselhotutelarv2.Models
{
    public class ChaveValidade
    {
        public int Cha_Id { get; set; }
        public string Cha_Chave {  get; set; }
        public DateTime Cha_Validade {  get; set; }
        public int Cha_IdEmpresa { get; set; }
        public Empresa Empresa { get; set; }
    }
}
