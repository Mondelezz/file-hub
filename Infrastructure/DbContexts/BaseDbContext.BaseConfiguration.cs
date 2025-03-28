using Domain.Common;

using Infrastructure.Configuration;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        EntityDateConfigure(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseConfiguration<>).Assembly);
    }

    private static void EntityDateConfigure(ModelBuilder modelBuilder)
    {
        var types = modelBuilder.Model.GetEntityTypes()
            .Where(t => t.ClrType.IsAssignableTo(typeof(IEntityDate)));

        foreach (var entity in types)
        {
            var property = entity.FindProperty("CreateDate")
                ?? throw new NullReferenceException();
            property.SetDefaultValueSql("timezone('utc', current_timestamp)");

            property = entity.FindProperty("UpdateDate")
                ?? throw new NullReferenceException();
            property.SetDefaultValueSql("timezone('utc', current_timestamp)");
        }
    }
}
