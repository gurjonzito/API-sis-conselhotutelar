using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class AtendimentoMap : IEntityTypeConfiguration<Atendimento>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Atendimento> builder)
        {
            builder.HasKey(x => x.Ate_Id);
            builder.Property(x => x.Ate_Codigo).IsRequired().HasMaxLength(11);
            builder.Property(x => x.Ate_Data).IsRequired();
            builder.Property(x => x.Ate_Status).IsRequired();
            builder.Property(x => x.Ate_Descritivo).HasMaxLength(255);

            builder.HasOne(c => c.Cidadao)
                   .WithMany()
                   .HasForeignKey(c => c.Ate_IdCliente)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Colaborador)
                   .WithMany()
                   .HasForeignKey(c => c.Ate_IdColaborador)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Ate_Codigo).IsUnique();
        }
    }
}
