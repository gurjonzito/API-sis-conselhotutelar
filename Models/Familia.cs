namespace API_sis_conselhotutelarv2.Models
{
    public class Familia
    {
        public int Fam_Id { get; set; }
        public string Fam_Sobrenomes { get; set; }
        public string Fam_Responsavel { get; set; }
        public int Fam_Participantes { get; set; }
        public int Ativo_Inativo { get; set; } = 1;
    }
}
