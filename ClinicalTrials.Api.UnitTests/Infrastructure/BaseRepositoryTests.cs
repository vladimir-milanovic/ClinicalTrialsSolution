using ClinicalTrials.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrials.UnitTests.Infrastructure;
public abstract class BaseRepositoryTests(DbContextOptions<ClinicalTrialsDbContext> options) : IDisposable
{
    protected readonly ClinicalTrialsDbContext _contextMock = new(options);

    protected BaseRepositoryTests(string databaseName)
        : this(new DbContextOptionsBuilder<ClinicalTrialsDbContext>()
            .UseInMemoryDatabase(databaseName: databaseName)
            .Options)
    {
    }

    public virtual void Dispose()
    {
        _contextMock.Dispose();
        GC.SuppressFinalize(this);
    }
}
