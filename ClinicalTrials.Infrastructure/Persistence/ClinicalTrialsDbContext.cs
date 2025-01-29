using ClinicalTrials.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.Infrastructure.Persistence;

public class ClinicalTrialsDbContext(DbContextOptions<ClinicalTrialsDbContext> options) : DbContext(options)
{
    public DbSet<ClinicalTrialMetadata> ClinicalTrialMetadata { get; set; }
}
