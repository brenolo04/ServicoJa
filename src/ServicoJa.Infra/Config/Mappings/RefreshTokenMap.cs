using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicoJa.Domain.Models;

namespace ServicoJa.Infra.Config.Mappings;

public class RefreshTokenMap : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshToken")
            .HasKey(r => r.Id);

        builder.Property(r => r.Token)
            .HasMaxLength(200);

        builder.HasIndex(r => r.Token)
            .IsUnique();

        builder.Property(r => r.ExpiresOnUtc)
            .HasColumnType("timestamp without time zone");
    }
}