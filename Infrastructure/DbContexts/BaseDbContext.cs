using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts;

public abstract partial class BaseDbContext : DbContext
{
    public DbSet<FileData> FileDatas { get; set; }
}
