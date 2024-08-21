namespace API_sis_conselhotutelarv2.Models
{
    public class Empresa
    {
        public int Emp_Id { get; set; }
        public string Emp_RazaoSocial { get; set; }
        public string? Emp_CNPJ { get; set; }
        public string Emp_Telefone {  get; set; }
        public int Ativo_Inativo { get; set; } = 1;
        public string Emp_Connection { get; set; }
    }
}
