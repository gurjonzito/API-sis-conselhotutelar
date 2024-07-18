using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class LogMap : IEntityTypeConfiguration<Logs>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Logs> builder)
        {
            builder.HasKey(x => x.Log_Id);
            builder.Property(c => c.Log_EmailUsuario).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Log_DataHora).IsRequired();
            builder.Property(c => c.Log_TipoOperacao).IsRequired().HasMaxLength(100);
        }
    }
}
