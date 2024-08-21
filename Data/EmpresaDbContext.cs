using API_sis_conselhotutelarv2.Data.Map;
using API_sis_conselhotutelarv2.Enums;
using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data
{
    public class EmpresaDbContext : DbContext
    {
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<ChaveValidade> ChavesValidade { get; set; }

        public EmpresaDbContext(DbContextOptions<EmpresaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmpresaMap());
            modelBuilder.ApplyConfiguration(new ChaveValidadeMap());

            modelBuilder.Entity<Empresa>(entity =>
            {
                entity.ToTable("Empresa");
            });

            modelBuilder.Entity<ChaveValidade>(entity =>
            {
                entity.ToTable("ChaveValidade");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
