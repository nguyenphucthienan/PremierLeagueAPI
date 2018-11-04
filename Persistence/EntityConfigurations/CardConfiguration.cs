using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class CardConfiguration : IEntityTypeConfiguration<Card>
    {
        public void Configure(EntityTypeBuilder<Card> builder)
        {
            builder.HasOne(c => c.Match)
                .WithMany(m => m.Cards)
                .HasForeignKey(c => c.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(c => c.Club)
                .WithMany(c => c.Cards)
                .HasForeignKey(c => c.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(c => c.Player)
                .WithMany(p => p.Cards)
                .HasForeignKey(c => c.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}