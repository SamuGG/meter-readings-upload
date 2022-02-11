using MeterReadings.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeterReadings.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<AccountEntity> Accounts { get; }
    DbSet<MeterReadingEntity> MeterReadings { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}