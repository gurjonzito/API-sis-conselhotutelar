using API_sis_conselhotutelarv2.Repositórios.Interfaces;

namespace API_sis_conselhotutelarv2.Repositórios
{
    public class StaticConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionString;

        public StaticConnectionStringProvider(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
