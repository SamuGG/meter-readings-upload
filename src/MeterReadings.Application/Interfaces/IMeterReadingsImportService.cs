using MeterReadings.Application.MeterReadings;
using MeterReadings.Application.Models;

namespace MeterReadings.Application.Interfaces;

public interface IMeterReadingsImportService
{
    Task<ImportServiceResult> ImportAsync(IEnumerable<MeterReadingModel> models, CancellationToken cancellationToken);
}