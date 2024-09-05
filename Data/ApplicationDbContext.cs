using Microsoft.EntityFrameworkCore;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Enums;
using API_sis_conselhotutelarv2.Repositórios.Interfaces;
using API_sis_conselhotutelarv2.Data.Map;

namespace API_sis_conselhotutelarv2.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly string _connectionString;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, string connectionString)
            : base(options)
        {
            _connectionString = connectionString;
        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Cliente> Cidadaos { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Logs> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrEmpty(_connectionString))
            {
                optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
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
