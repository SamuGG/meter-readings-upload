using MediatR;
using MeterReadings.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MeterReadings.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient<IMeterReadingsImportService, MeterReadings.ImportService>();
        return services;
    }
}