using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class SquadPlayerConfiguration : IEntityTypeConfiguration<SquadPlayer>
    {
        public void Configure(EntityTypeBuilder<SquadPlayer> builder)
        {
            builder.HasKey(sp => new {sp.SquadId, sp.PlayerId});

            builder.HasOne(sp => sp.Squad)
                .WithMany(s => s.SquadPlayers)
                .HasForeignKey(sp => sp.SquadId)
                .IsRequired();

            builder.HasOne(sp => sp.Player)
                .WithMany(s => s.SquadPlayers)
                .HasForeignKey(sp => sp.PlayerId)
                .IsRequired();
        }
    }
}