using MeterReadings.Domain.Entities;
using MeterReadings.Infrastructure.Files;
using MeterReadings.Infrastructure.Files.Mappings;

namespace MeterReadings.Infrastructure.Persistence;

public class ApplicationDbInitializer
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbInitializer(ApplicationDbContext context) => _context = context;

    public async Task InitializeAsync(string accountsFilename)
    {
        if (!_context.Accounts.Any())
        {
            await _context.Accounts.AddRangeAsync(CsvFileReader<AccountEntity, AccountEntityMap>.ReadAll(accountsFilename));
            await _context.SaveChangesAsync();
        }
    }
}