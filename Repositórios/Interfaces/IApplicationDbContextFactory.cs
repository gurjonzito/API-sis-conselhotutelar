using API_sis_conselhotutelarv2.Data;

namespace API_sis_conselhotutelarv2.Repositórios.Interfaces
{
    public interface IApplicationDbContextFactory
    {
        ApplicationDbContext CreateDbContext(string connectionString);
    }
}
