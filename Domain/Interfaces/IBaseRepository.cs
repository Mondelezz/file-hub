namespace Domain.Interfaces;

public interface IBaseRepository<T> where T : class
{
    /// <summary>
    /// Добавляет сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Добавленная сущность</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken);

    /// <summary>
    /// Получает сущность по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Сущность</returns>
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken);
}
