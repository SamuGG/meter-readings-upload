namespace MeterReadings.Domain.Entities;

public class AccountEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public IList<MeterReadingEntity> MeterReadings { get; set; } = new List<MeterReadingEntity>();
}