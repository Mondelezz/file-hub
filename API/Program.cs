using API;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

await builder
    .ConfigureServices()
    .Build()
    .ConfigurePipeline()
    .RunAsync();
