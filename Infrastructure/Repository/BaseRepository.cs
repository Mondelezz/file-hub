using Domain.Interfaces;

using Infrastructure.DbContexts;

namespace Infrastructure.Repository;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly ApplicationDbContext dbContext;

    public BaseRepository(ApplicationDbContext dbContext) => this.dbContext = dbContext;

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
    {
        await dbContext.Set<T>().AddAsync(entity, cancellationToken);

        return entity;
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken) =>
        await dbContext.Set<T>().FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
}
