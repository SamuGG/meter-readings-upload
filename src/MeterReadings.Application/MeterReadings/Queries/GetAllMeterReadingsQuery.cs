using MediatR;
using MeterReadings.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Application.MeterReadings.Queries;

public record GetAllMeterReadingsRequest() : IRequest<IEnumerable<QueryMeterReadingModel>>;
public record QueryMeterReadingModel(int AccountId, string FirstName, string LastName, DateTime ReadingDateTime, int ReadingValue);

public class GetAllMeterReadingsRequestHandler : IRequestHandler<GetAllMeterReadingsRequest, IEnumerable<QueryMeterReadingModel>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMeterReadingsRequestHandler(IApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<QueryMeterReadingModel>> Handle(GetAllMeterReadingsRequest request, CancellationToken cancellationToken) =>
        await _context.MeterReadings
            .AsNoTracking()
            .Include(meterReading => meterReading.Account)
            .Select(meterReading => 
                new QueryMeterReadingModel(
                    meterReading.AccountId, 
                    meterReading.Account!.FirstName, 
                    meterReading.Account.LastName, 
                    meterReading.ReadingDateTime, 
                    meterReading.ReadingValue))
            .OrderByDescending(meterReading => meterReading.ReadingDateTime)
            .ToArrayAsync(cancellationToken);
}