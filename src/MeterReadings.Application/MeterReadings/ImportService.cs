using MediatR;
using MeterReadings.Application.Interfaces;
using MeterReadings.Application.MeterReadings.Commands;
using MeterReadings.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Application.MeterReadings;

public class ImportService : IMeterReadingsImportService
{
    private readonly IApplicationDbContext _context;
    private readonly ISender _mediator;

    public ImportService(IApplicationDbContext context, ISender mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<ImportServiceResult> ImportAsync(IEnumerable<MeterReadingModel> models, CancellationToken cancellationToken = default)
    {
        ImportServiceResult result = new();
        var modelsEnumerator = models.GetEnumerator();

        while (modelsEnumerator.MoveNext() && !cancellationToken.IsCancellationRequested)
        {
            if (!modelsEnumerator.Current.HasValidReadingValue)
            {
                result.Failure();
                continue;
            }

            try
            {
                var meterReadingEntity = _context.MeterReadings
                    .Include(meterReading => meterReading.Account)
                    .FirstOrDefault(meterReading => meterReading.AccountId == modelsEnumerator.Current.AccountId);

                if (meterReadingEntity == null)
                {
                    await _mediator.Send(new CreateNewRecordRequest(modelsEnumerator.Current), cancellationToken);
                    result.Success();
                    continue;
                }

                if (meterReadingEntity.IsNewerThan(modelsEnumerator.Current.ReadingDateTime))
                {
                    result.Failure();
                    continue;
                }

                await _mediator.Send(new UpdateRecordRequest(meterReadingEntity, modelsEnumerator.Current), cancellationToken);
                result.Success();
            }
            catch
            {
                result.Failure();
            }
        }

        return result;
    }
}