using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class ClubConfiguration : IEntityTypeConfiguration<Club>
    {
        public void Configure(EntityTypeBuilder<Club> builder)
        {
            builder.HasIndex(c => c.Code)
                .IsUnique();

            builder.Property(c => c.Code)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(c => c.Name)
                .IsRequired();
        }
    }
}