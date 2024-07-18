using Microsoft.EntityFrameworkCore;
using API_sis_conselhotutelarv2.Models;
using API_sis_conselhotutelarv2.Data.Map;

namespace API_sis_conselhotutelarv2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        
        }

        public DbSet<Colaborador> Colaboradores { get; set; }
        public DbSet<Cliente> Cidadaos { get; set; }
        public DbSet<Atendimento> Atendimentos { get; set; }
        public DbSet<Familia> Familias { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Logs> Logs { get; set; }

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

            base.OnModelCreating(modelBuilder);
        }
    }
}
