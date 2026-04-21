using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoJa.Domain.Models;

namespace ServicoJa.Infra.Config.Mappings;

public class ServicoMap : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos")
            .HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd();
        
        builder.Property(s => s.IdPerfil)
            .IsRequired();

        builder.Property(s => s.Nome)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Property(s => s.Descricao)
            .IsRequired()
            .HasColumnType("varchar")
            .HasMaxLength(1000);

        builder.Property(s => s.Valor)
            .IsRequired()
            .HasColumnType("numeric(10,2)");
            
        builder.Property(s => s.Inativo)
            .IsRequired()
            .HasColumnType("boolean")
            .HasDefaultValue(false);

        builder.Property(s => s.DataCriado)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        builder.HasOne(s => s.Perfil)
            .WithMany(p => p.Servicos)
            .HasForeignKey(s => s.IdPerfil)
            .OnDelete(DeleteBehavior.NoAction);
    }
}