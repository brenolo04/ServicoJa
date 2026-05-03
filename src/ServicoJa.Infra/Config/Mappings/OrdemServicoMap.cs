using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoJa.Domain.Models;

namespace ServicoJa.Infra.Config.Mappings;

public class OrdemServicoMap : IEntityTypeConfiguration<OrdemServico>
{
    public void Configure(EntityTypeBuilder<OrdemServico> builder)
    {
        builder.ToTable("OrdemServicos")
            .HasKey(o => o.Id);

        builder.Property(s => s.Id)
            .UseIdentityAlwaysColumn()
            .ValueGeneratedOnAdd();

        builder.Property(o => o.IdServico)
            .IsRequired();
            
        builder.Property(o => o.IdPerfilPrestador)
            .IsRequired();
            
        builder.Property(o => o.IdPerfilSolicitante)
            .IsRequired(false);

        builder.Property(o => o.NomeSolicitante)
            .IsRequired(false)
            .HasColumnType("varchar")
            .HasMaxLength(100);

        builder.Property(o => o.SolicitanteAnonimo)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(o => o.DataMarcado)
            .IsRequired()
            .HasColumnType("timestamp without time zone");
            
        builder.Property(o => o.DataFinalizado)
            .IsRequired(false)
            .HasColumnType("timestamp without time zone");
            
        builder.Property(o => o.DataCriacao)
            .IsRequired()
            .HasColumnType("timestamp without time zone");
            
        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.OwnsOne(o => o.Endereco, endereco =>
        {
            endereco.Property(e => e.Cep)
                .IsRequired()
                .HasColumnName("Cep")
                .HasColumnType("varchar")
                .HasMaxLength(8);

            endereco.Property(e => e.Cidade)
                .IsRequired()
                .HasColumnName("Cidade")
                .HasColumnType("varchar")
                .HasMaxLength(50);

            endereco.Property(e => e.Bairro)
                .IsRequired()
                .HasColumnName("Bairro")
                .HasColumnType("varchar")
                .HasMaxLength(50);

            endereco.Property(e => e.Rua)
                .IsRequired()
                .HasColumnName("Rua")
                .HasColumnType("varchar")
                .HasMaxLength(50);

            endereco.Property(e => e.Numero)
                .IsRequired(false)
                .HasColumnName("Numero")
                .HasColumnType("varchar")
                .HasMaxLength(10);
        });

        builder.HasOne(o => o.PerfilPrestador)
            .WithMany()
            .HasForeignKey(o => o.IdPerfilPrestador)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(o => o.PerfilSolicitante)
            .WithMany()
            .HasForeignKey(o => o.IdPerfilSolicitante)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasOne(o => o.Servico)
            .WithMany()
            .HasForeignKey(o => o.IdServico)
            .OnDelete(DeleteBehavior.Restrict);
    }
}