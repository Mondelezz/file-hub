using Microsoft.OpenApi.Models;
using Infrastructure;
using Application;
using Minio;
using Application.Options;

namespace API;

internal static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Configuration
           .SetBasePath(AppContext.BaseDirectory)
           .AddJsonFile(Path.Combine("appsettings.json"), optional: false, reloadOnChange: true);

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .WithExposedHeaders("*");
            });
        });

        builder.Services.Configure<S3Options>(
            builder.Configuration.GetSection("S3Options"));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Fun&Sun",
                Version = "v1",
                Contact = new OpenApiContact
                {
                    Url = new Uri("https://github.com/Mondelezz"),
                    Name = "Egor Pankov"
                },
            });

            foreach (string xmlFile in Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.AllDirectories))
            {
                options.IncludeXmlComments(xmlFile, true);
            }

            options.CustomSchemaIds(type => type.FullName?.Replace("+", "_"));
        });

        S3Options s3Options = builder.Configuration
                .GetSection(nameof(S3Options))
                .Get<S3Options>() ?? throw new ArgumentException(nameof(S3Options));

        builder.Services.AddMinio(opt => opt
            .WithEndpoint(s3Options.Endpoint)
            .WithSSL(false)
            .WithCredentials(s3Options.AccessKey, s3Options.SecretKey));

        builder.Services.RegisterInfrastructureLayer(builder.Configuration, builder.Environment);
        builder.Services.RegisterApplicationLayer(builder.Configuration);

        return builder;
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseCors("AllowAll");

        app.UseRouting();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());

        return app;
    }
}
