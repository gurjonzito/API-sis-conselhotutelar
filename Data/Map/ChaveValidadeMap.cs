using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class ChaveValidadeMap : IEntityTypeConfiguration<ChaveValidade>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ChaveValidade> builder)
        {
            builder.HasKey(x => x.Cha_Id);
            builder.Property(x => x.Cha_Chave).IsRequired().HasMaxLength(8);
            builder.HasIndex(x => x.Cha_Chave).IsUnique();

            builder.HasOne(c => c.Empresa)
                   .WithMany()
                   .HasForeignKey(c => c.Cha_IdEmpresa)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
