using CsvHelper.Configuration;
using MeterReadings.Application.Models;

namespace MeterReadings.Infrastructure.Files.Mappings;

public class MeterReadingModelMap : ClassMap<MeterReadingModel>
{
    public MeterReadingModelMap()
    {
        Map(model => model.AccountId).Index(0);
        Map(model => model.ReadingDateTime)
            .TypeConverterOption.Format("dd/MM/yyyy HH:mm")
            .Index(1);
        Map(model => model.ReadingValue).Index(2);
    }
}