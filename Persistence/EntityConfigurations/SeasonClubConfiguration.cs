using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class SeasonClubConfiguration : IEntityTypeConfiguration<SeasonClub>
    {
        public void Configure(EntityTypeBuilder<SeasonClub> builder)
        {
            builder.ToTable("SeasonClubs");
            builder.HasKey(sc => new {sc.SeasonId, sc.ClubId});

            builder.HasOne(sc => sc.Season)
                .WithMany(s => s.SeasonClubs)
                .HasForeignKey(sc => sc.SeasonId)
                .IsRequired();

            builder.HasOne(sc => sc.Club)
                .WithMany(s => s.SeasonClubs)
                .HasForeignKey(sc => sc.ClubId)
                .IsRequired();
        }
    }
}