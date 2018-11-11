using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class SeasonConfiguration : IEntityTypeConfiguration<Season>
    {
        public void Configure(EntityTypeBuilder<Season> builder)
        {
            builder.Property(c => c.Name)
                .IsRequired();

            builder.HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}