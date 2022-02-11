using MeterReadings.Application;
using MeterReadings.Infrastructure;
using MeterReadings.Infrastructure.Persistence;
using MeterReadings.WebAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddTransient<ApplicationDbInitializer>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<ApplicationDbInitializer>()
        .InitializeAsync(builder.Configuration.GetValue<string>("AccountsSeedFilename"));
}

app.UseApiEndpoints();
app.Run();