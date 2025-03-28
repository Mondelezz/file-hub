using Domain.Common;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions)
    : BaseDbContext(contextOptions)
{
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateTimestamps()
    {
        IEnumerable<IEntityDate?> entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Modified && e.Entity is IEntityDate)
            .Select(e => e.Entity as IEntityDate);

        foreach (IEntityDate? entry in entries)
        {
            if (entry is null)
            {
                continue;
            }

            entry.UpdateDate = DateTime.UtcNow;
        }

        entries = ChangeTracker
             .Entries()
             .Where(e => e.State == EntityState.Added && e.Entity is IEntityDate)
             .Select(e => e.Entity as IEntityDate);

        foreach (IEntityDate? entry in entries)
        {
            if (entry is null)
            {
                continue;
            }

            entry.UpdateDate = DateTime.UtcNow;
            entry.CreateDate = DateTime.UtcNow;
        }
    }
}
