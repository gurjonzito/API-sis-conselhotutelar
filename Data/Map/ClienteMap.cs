using API_sis_conselhotutelarv2.Models;
using Microsoft.EntityFrameworkCore;

namespace API_sis_conselhotutelarv2.Data.Map
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(x => x.Cli_Id);
            builder.Property(x => x.Cli_Nome).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Cli_Email).HasMaxLength(100);
            builder.Property(x => x.Cli_Telefone).HasMaxLength(20);

            builder.HasOne(c => c.Familia)
                   .WithMany()
                   .HasForeignKey(c => c.Cli_IdFamilia)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
