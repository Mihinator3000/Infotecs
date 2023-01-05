using System.Text;
using Infotecs.Domain.Models;
using Infotecs.Tests.Extensions;

namespace Infotecs.Tests.Tools;

public class CsvTools
{
    private const string DateTimeFormat = "yyyy-MM-dd_HH-mm-ss";
    private readonly IFormatProvider _formatProvider;

    public CsvTools(IFormatProvider formatProvider)
    {
        _formatProvider = formatProvider;
    }

    public Stream CreateCsvStream(IReadOnlyCollection<Value> values)
    {
        var stringBuilder = new StringBuilder();

        foreach (Value value in values)
        {
            string csvValueString = CreateCsvValueString(value);
            stringBuilder.AppendLine(csvValueString);
        }

        return stringBuilder.ToString().ToStream();
    }

    public string CreateCsvValueString(Value value)
    {
        return $"{value.DateTime.ToString(DateTimeFormat)};{value.TimeInSeconds};{value.Rate.ToString(_formatProvider)}";
    }
}