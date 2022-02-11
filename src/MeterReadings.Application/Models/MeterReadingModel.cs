using System.Text.RegularExpressions;

namespace MeterReadings.Application.Models;

public record MeterReadingModel
{
    private string? _readingValue;
    private bool _hasValidReadingValue;
    private int _numericReadingValue;

    public int AccountId { get; set; }
    public DateTime ReadingDateTime { get; set; }
    public string? ReadingValue
    {
        get => _readingValue;
        set
        {
            _readingValue = value;
            _hasValidReadingValue = TryConvertReadingValueToNumeric(out _numericReadingValue);
        }
    }
    public bool HasValidReadingValue => _hasValidReadingValue;

    public int GetNumericReadingValue()
    {
        if (!HasValidReadingValue)
            throw new InvalidOperationException();

        return _numericReadingValue;
    }

    private bool TryConvertReadingValueToNumeric(out int numericValue)
    {
        const int maxLengthAllowed = 5;
        const string regexFormat = @"^[0-9]+$";

        numericValue = default;
        string spanToParse = ReadingValue?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(spanToParse) || spanToParse.Length > maxLengthAllowed)
            return false;

        return Regex.IsMatch(spanToParse, regexFormat) && int.TryParse(spanToParse, out numericValue);
    }
}