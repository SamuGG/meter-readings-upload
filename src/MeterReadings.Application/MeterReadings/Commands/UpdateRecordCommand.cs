using MediatR;
using MeterReadings.Application.Exceptions;
using MeterReadings.Application.Interfaces;
using MeterReadings.Application.Models;
using MeterReadings.Domain.Entities;

namespace MeterReadings.Application.MeterReadings.Commands;

public record UpdateRecordRequest(MeterReadingEntity MeterReadingEntity, MeterReadingModel MeterReadingModel) : IRequest;

public class UpdateRecordRequestHandler : IRequestHandler<UpdateRecordRequest>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecordRequestHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(UpdateRecordRequest request, CancellationToken cancellationToken)
    {
        if (request.MeterReadingEntity.Account == null)
            throw new AccountNotFoundException(request.MeterReadingEntity.AccountId);

        request.MeterReadingEntity.ReadingDateTime = request.MeterReadingModel.ReadingDateTime;
        request.MeterReadingEntity.ReadingValue = request.MeterReadingModel.GetNumericReadingValue();
        
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}