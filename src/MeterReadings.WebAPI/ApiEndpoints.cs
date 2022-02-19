using MediatR;
using MeterReadings.Application.Interfaces;
using MeterReadings.Application.MeterReadings.Queries;
using MeterReadings.Application.Models;
using MeterReadings.Infrastructure.Files;
using MeterReadings.Infrastructure.Files.Mappings;

namespace MeterReadings.WebAPI;

public static class ApiEndpoints
{
    public static IEndpointRouteBuilder UseApiEndpoints(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/meter-reading-downloads", async (ISender mediator) => 
            await mediator.Send(new GetAllMeterReadingsRequest()))
        .Produces<IEnumerable<QueryMeterReadingModel>>();

        builder.MapPost("/meter-reading-uploads", async (HttpContext httpContext, IMeterReadingsImportService service, CancellationToken cancellationToken) =>
        {
            if (httpContext.Request.Form.Files.Count != 1)
                return Results.BadRequest();
            
            string tempFilename = Path.GetTempFileName();
            await using FileStream stream = new(tempFilename, FileMode.Create);
            await httpContext.Request.Form.Files[0].OpenReadStream().CopyToAsync(stream, cancellationToken);
            await stream.FlushAsync();

            var result = await service.ImportAsync(
                CsvFileReader<MeterReadingModel, MeterReadingModelMap>.ReadAll(tempFilename),
                cancellationToken);

            File.Delete(tempFilename);
            return Results.Ok(result);
        })
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<Application.MeterReadings.ImportServiceResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest);

        return builder;
    }
}