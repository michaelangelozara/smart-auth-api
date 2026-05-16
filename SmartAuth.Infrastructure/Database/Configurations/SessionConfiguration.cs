using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartAuth.Domain.Sessions;

namespace SmartAuth.Infrastructure.Database.Configurations;

public sealed class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.AccessToken)
            .HasMaxLength(3000)
            .IsRequired();

        builder.Property(x => x.AccessTokenExpiration)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.Property(x => x.RefreshToken)
            .HasMaxLength(3000)
            .IsRequired();   

        builder.Property(x => x.RefreshTokenExpiration)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.HasOne(x => x.User)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
