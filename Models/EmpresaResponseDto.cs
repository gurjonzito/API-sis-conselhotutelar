namespace API_sis_conselhotutelarv2.Models
{
    public class EmpresaResponseDto<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public string ChaveDeValidade { get; set; }
    }
}
