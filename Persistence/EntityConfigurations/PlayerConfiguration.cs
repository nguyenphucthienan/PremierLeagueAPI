using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();

            builder.HasIndex(c => c.Name);
        }
    }
}