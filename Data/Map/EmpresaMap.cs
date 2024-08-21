using API_sis_conselhotutelarv2.Enums;
using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class EmpresaMap : IEntityTypeConfiguration<Empresa>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Empresa> builder)
        {
            builder.HasKey(x => x.Emp_Id);
            builder.Property(x => x.Emp_RazaoSocial).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Emp_CNPJ).HasMaxLength(14);
            builder.HasIndex(x => x.Emp_CNPJ).IsUnique();
            builder.Property(x => x.Emp_Telefone).HasMaxLength(20);
            builder.Property(x => x.Ativo_Inativo).IsRequired().HasDefaultValue(1);
            builder.Property(x => x.Emp_Connection).IsRequired().HasMaxLength(255);
        }
    }
}
