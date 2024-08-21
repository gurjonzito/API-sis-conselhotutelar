using API_sis_conselhotutelarv2.Enums;
using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class FamiliaMap : IEntityTypeConfiguration<Familia>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Familia> builder)
        {
            builder.HasKey(x => x.Fam_Id);
            builder.Property(c => c.Fam_Sobrenomes).IsRequired().HasMaxLength(150);
            builder.Property(c => c.Fam_Responsavel).IsRequired().HasMaxLength(150);
            builder.Property(c => c.Fam_Participantes).IsRequired();
            builder.Property(x => x.Ativo_Inativo).IsRequired().HasDefaultValue(1);
        }
    }
}
