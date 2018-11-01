using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Models;

namespace PremierLeagueAPI.Configurations
{
    public class GoalConfiguration : IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder.HasOne(g => g.Match)
                .WithMany(m => m.Goals)
                .HasForeignKey(g => g.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(g => g.Club)
                .WithMany(c => c.Goals)
                .HasForeignKey(g => g.ClubId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(g => g.Player)
                .WithMany(p => p.Goals)
                .HasForeignKey(g => g.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}