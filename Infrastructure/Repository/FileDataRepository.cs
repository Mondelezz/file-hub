using Application.Interfaces;

using Domain.Models;

using Infrastructure.DbContexts;

namespace Infrastructure.Repository;

public class FileDataRepository : BaseRepository<FileData>, IFileDataRepository
{
    public FileDataRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<FileData> AddDataAsync(FileData fileData, CancellationToken cancellationToken)
    {
        await AddAsync(fileData, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return fileData;
    }

    public async Task<FileData?> GetFileDataByIdAsync(long fileId, CancellationToken cancellationToken) =>
        await GetByIdAsync(fileId, cancellationToken);
}
