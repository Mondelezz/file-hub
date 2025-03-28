using Application.Interfaces;

using Infrastructure.DbContexts;
using Infrastructure.Repository;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Npgsql;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        string connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new NullReferenceException("ConnectionString to database is null");

        NpgsqlDataSourceBuilder dataSourceBuilder = new(connectionString);

        NpgsqlDataSource dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(dataSource)
            .DevelopmentEnableSensitiveData(environment);
        });

        services.AddScoped<IFileDataRepository, FileDataRepository>();

        return services;
    }

    private static DbContextOptionsBuilder DevelopmentEnableSensitiveData(
        this DbContextOptionsBuilder optionsBuilder,
        IHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            optionsBuilder.EnableSensitiveDataLogging(true)
                          .EnableDetailedErrors();
        }

        return optionsBuilder;
    }
}
