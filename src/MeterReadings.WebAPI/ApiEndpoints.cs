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

        builder.MapPost("/meter-reading-uploads", async (IFormFile formFile, IMeterReadingsImportService service, CancellationToken cancellationToken) =>
        {
            string tempFilename = Path.GetTempFileName();
            await using FileStream stream = new(tempFilename, FileMode.Create);
            await formFile.OpenReadStream().CopyToAsync(stream, cancellationToken);

            var result = await service.ImportAsync(
                CsvFileReader<MeterReadingModel, MeterReadingModelMap>.ReadAll(tempFilename),
                cancellationToken);

            File.Delete(tempFilename);
            return result;
        })
        .Accepts<IFormFile>("multipart/form-data")
        .Produces<Application.MeterReadings.ImportServiceResult>();

        // Uncomment alternative endpoint version if uploading the file fails
        //builder.MapPost("/meter-reading-uploads", async (IMeterReadingsImportService service, CancellationToken cancellationToken) =>
        //{
        //    string tempFilename = "..\\..\\csv\\Meter_Reading.csv";

        //    var result = await service.ImportAsync(
        //        CsvFileReader<MeterReadingModel, MeterReadingModelMap>.ReadAll(tempFilename),
        //        cancellationToken);

        //    return result;
        //})
        //.Accepts<IFormFile>("multipart/form-data")
        //.Produces<Application.MeterReadings.ImportServiceResult>();

        return builder;
    }
}