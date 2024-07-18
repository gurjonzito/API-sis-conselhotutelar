using Microsoft.EntityFrameworkCore;
using API_sis_conselhotutelarv2.Models;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class ColaboradorMap : IEntityTypeConfiguration<Colaborador>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Colaborador> builder)
        {
            builder.HasKey(x => x.Col_Id);
            builder.Property(x => x.Col_Nome).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Col_Username).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Col_Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Col_Telefone).HasMaxLength(20);
            builder.Property(x => x.Col_Senha).HasMaxLength(1000);

            builder.HasOne(c => c.Cargo)
                   .WithMany()
                   .HasForeignKey(c => c.Col_IdCargo)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
