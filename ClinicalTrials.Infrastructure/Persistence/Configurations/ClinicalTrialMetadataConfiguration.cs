using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicalTrials.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicalTrials.Infrastructure.Persistence.Configurations;

public class ClinicalTrialMetadataConfiguration : IEntityTypeConfiguration<ClinicalTrialMetadata>
{
    public void Configure(EntityTypeBuilder<ClinicalTrialMetadata> builder)
    {
        builder.HasKey(e => e.TrialId);

        builder.Property(e => e.TrialId)
            .IsRequired();

        builder.Property(e => e.Title)
            .IsRequired();

        builder.Property(e => e.StartDate)
            .IsRequired();

        builder.Property(e => e.EndDate);

        builder.Property(e => e.Participants)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.Duration);
    }
}
