using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PremierLeagueAPI.Models;

namespace PremierLeagueAPI.Configurations
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