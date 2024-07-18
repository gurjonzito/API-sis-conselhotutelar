using API_sis_conselhotutelarv2.Enums;
using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class CargoMap : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cargo> builder)
        {
            builder.HasKey(x => x.Car_Id);
            builder.Property(c => c.Car_Nome)
                           .IsRequired()
                           .HasMaxLength(100)
                           .HasConversion<string>() // Converte enum para string (opcional, depende da sua estratégia de armazenamento)
                           .HasDefaultValue(NomeCargo.Administrador | NomeCargo.Conselheiro); // Valor padrão
            builder.HasIndex(c => c.Car_Nome).IsUnique();
        }
    }
}
