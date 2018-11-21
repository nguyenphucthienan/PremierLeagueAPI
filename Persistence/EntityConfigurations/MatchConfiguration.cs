using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder.ToTable("Matches");

            builder.HasOne(m => m.HomeClub)
                .WithMany(c => c.HomeMatches)
                .HasForeignKey(m => m.HomeClubId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(m => m.AwayClub)
                .WithMany(c => c.AwayMatches)
                .HasForeignKey(m => m.AwayClubId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(m => m.HomeKit)
                .WithMany(k => k.HomeMatches)
                .HasForeignKey(m => m.HomeKitId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(m => m.AwayKit)
                .WithMany(k => k.AwayMatches)
                .HasForeignKey(m => m.AwayKitId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Ignore(m => m.HomeScore);
            builder.Ignore(m => m.AwayScore);
        }
    }
}