namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string chaveValidade);
    }
}
