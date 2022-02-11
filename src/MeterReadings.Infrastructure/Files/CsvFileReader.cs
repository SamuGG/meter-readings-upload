using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace MeterReadings.Infrastructure.Files;

public static class CsvFileReader<T, M> where M : ClassMap
{
    public static IEnumerable<T> ReadAll(string filename)
    {
        using var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        csv.Context.RegisterClassMap<M>();
        return csv.GetRecords<T>().ToArray();
    }
}