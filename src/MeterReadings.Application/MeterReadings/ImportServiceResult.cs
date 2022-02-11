namespace MeterReadings.Application.MeterReadings;

public record ImportServiceResult
{
    public int SuccessfulRecordCount { get; private set; }
    public int FailedRecordCount { get; private set; }

    public void Success() => SuccessfulRecordCount++;
    public void Failure() => FailedRecordCount++;
}