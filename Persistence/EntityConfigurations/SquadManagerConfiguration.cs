using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Core.Models;

namespace PremierLeagueAPI.Persistence.EntityConfigurations
{
    public class SquadManagerConfiguration : IEntityTypeConfiguration<SquadManager>
    {
        public void Configure(EntityTypeBuilder<SquadManager> builder)
        {
            builder.ToTable("SquadManagers");
            builder.HasKey(sm => new { sm.SquadId, sm.ManagerId});

            builder.HasOne(sm => sm.Squad)
                .WithMany(s => s.SquadManagers)
                .HasForeignKey(sm => sm.SquadId)
                .IsRequired();

            builder.HasOne(sm => sm.Manager)
                .WithMany(s => s.SquadManagers)
                .HasForeignKey(sm => sm.ManagerId)
                .IsRequired();
        }
    }
}