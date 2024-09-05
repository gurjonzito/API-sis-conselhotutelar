using API_sis_conselhotutelarv2.Data;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class ApplicationDbContextFactory : IApplicationDbContextFactory
    {
        private readonly IConnectionStringProvider _connectionStringProvider;

        public ApplicationDbContextFactory(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public ApplicationDbContext CreateDbContext(string chaveValidade)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = _connectionStringProvider.GetConnectionString(chaveValidade);

            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

            return new ApplicationDbContext(optionsBuilder.Options, new StaticConnectionStringProvider(connectionString));
        }
    }
}
