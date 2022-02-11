using MediatR;
using MeterReadings.Application.Exceptions;
using MeterReadings.Application.Interfaces;
using MeterReadings.Application.Models;
using MeterReadings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Application.MeterReadings.Commands;

public record CreateNewRecordRequest(MeterReadingModel MeterReadingModel) : IRequest;

public class CreateNewRecordRequestHandler : IRequestHandler<CreateNewRecordRequest>
{
    private readonly IApplicationDbContext _context;

    public CreateNewRecordRequestHandler(IApplicationDbContext context) => _context = context;

    public async Task<Unit> Handle(CreateNewRecordRequest request, CancellationToken cancellationToken)
    {
        if (!await _context.Accounts
            .AsNoTracking()
            .AnyAsync(account => account.Id == request.MeterReadingModel.AccountId, cancellationToken))
            throw new AccountNotFoundException(request.MeterReadingModel.AccountId);

        await _context.MeterReadings.AddAsync(
            new MeterReadingEntity()
            {
                AccountId = request.MeterReadingModel.AccountId,
                ReadingDateTime = request.MeterReadingModel.ReadingDateTime,
                ReadingValue = request.MeterReadingModel.GetNumericReadingValue()
            },
            cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}