using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoJa.Domain.Models;

namespace ServicoJa.Infra.Config.Mappings;

public class PerfilMap : IEntityTypeConfiguration<Perfil>
{
    public void Configure(EntityTypeBuilder<Perfil> builder)
    {
        builder.ToTable("Perfis")
            .HasKey(o => o.Id);

        builder.Property(s => s.Id)
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd();

        builder.Property(p => p.IdUsuarioIdentity)
            .IsRequired();

        builder.HasOne<UsuarioIdentity>()
            .WithMany()
            .HasForeignKey(p => p.IdUsuarioIdentity)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(p => p.Nome)
            .HasColumnName("nome")
            .HasColumnType("varchar")
            .HasMaxLength(80);
    }
}