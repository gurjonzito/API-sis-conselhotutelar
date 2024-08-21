using Microsoft.EntityFrameworkCore;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Enums;
using Microsoft.AspNetCore.Http;
using API_sis_conselhotutelarv2.Data.Map;
using Microsoft.AspNetCore.Authentication;

namespace API_sis_conselhotutelarv2.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly EmpresaDbContext _principalDbContext;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor, EmpresaDbContext principalDbContext)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _principalDbContext = principalDbContext;
        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Cliente> Cidadaos { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Logs> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("ChaveValidade", out var chaveValidade))
            {
                var empresa = _principalDbContext.ChavesValidade
                    .Include(c => c.Empresa)
                    .FirstOrDefault(c => c.Cha_Chave == chaveValidade && c.Cha_Validade >= DateTime.Now);

                string connectionString = empresa?.Empresa.Emp_Connection;

                if (!string.IsNullOrEmpty(connectionString))
                {
                    optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                }
                else
                {
                    throw new Exception("Chave de validade inválida ou expirada.");
                }
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ColaboradorMap());
            modelBuilder.ApplyConfiguration(new AtendimentoMap());
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new CargoMap());
            modelBuilder.ApplyConfiguration(new FamiliaMap());
            modelBuilder.ApplyConfiguration(new EmpresaMap());
            modelBuilder.ApplyConfiguration(new LogMap());

            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.ToTable("Cargo");
            });

            modelBuilder.Entity<Colaborador>(entity =>
            {
                entity.ToTable("Colaborador");
            });

            modelBuilder.Entity<Atendimento>(entity =>
            {
                entity.ToTable("Atendimento");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Cliente");
            });

            modelBuilder.Entity<Familia>(entity =>
            {
                entity.ToTable("Familia");
            });

            modelBuilder.Entity<Atendimento>(entity =>
            {
                entity.Property(e => e.Ate_Status)
                    .HasConversion(
                        v => v.ToString(),
                        v => (StatusAtendimento)Enum.Parse(typeof(StatusAtendimento), v)
                    );
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
