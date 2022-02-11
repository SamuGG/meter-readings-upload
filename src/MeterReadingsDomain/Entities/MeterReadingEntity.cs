namespace MeterReadings.Domain.Entities;

public class MeterReadingEntity
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime ReadingDateTime { get; set; }
    public int ReadingValue { get; set; }
    public AccountEntity? Account { get; set; }

    public bool IsNewerThan(DateTime comparisonDateTime) => ReadingDateTime > comparisonDateTime;
}
