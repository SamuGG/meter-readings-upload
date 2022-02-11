using CsvHelper.Configuration;
using MeterReadings.Domain.Entities;

namespace MeterReadings.Infrastructure.Files.Mappings;

public class AccountEntityMap : ClassMap<AccountEntity>
{
    public AccountEntityMap()
    {
        Map(model => model.Id).Index(0);
        Map(model => model.FirstName).Index(1);
        Map(model => model.LastName).Index(2);
    }
}