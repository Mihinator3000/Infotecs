using System.Globalization;
using FluentValidation;
using FluentValidation.Results;
using Infotecs.Abstractions.Core.Parsers;
using Infotecs.Abstractions.Core.Providers;
using Infotecs.Domain.Exceptions;
using Infotecs.Domain.Models;
using Infotecs.Domain.ValueTypes;
using Microsoft.VisualBasic.FileIO;

namespace Infotecs.Core.Parsers;

public class CsvParser : ICsvParser
{
    private static readonly string[] CsvDelimiters = { ";" };

    private const int FieldsCount = 3;

    private const string DateTimeFormat = "yyyy-MM-dd_HH-mm-ss";

    private const int MinLineCount = 1;
    private const int MaxLineCount = 10000;

    private readonly ICultureProvider _cultureProvider;
    private readonly IValidator<Value> _valueValidator;

    public CsvParser(ICultureProvider cultureProvider, IValidator<Value> valueValidator)
    {
        _cultureProvider = cultureProvider;
        _valueValidator = valueValidator;
    }

    public IReadOnlyCollection<Value> Parse(FileData fileData)
    {
        using var parser = CreateFieldParser(fileData.ReadStream);

        var values = new List<Value>();

        while (!parser.EndOfData)
        {
            long lineNumber = parser.LineNumber;

            if (lineNumber > MaxLineCount)
            {
                throw new CsvParseException("Exceeded limit of lines in a file");
            }

            string[]? fields = parser.ReadFields();

            if (fields is null)
            {
                throw new CsvParseException($"Line {lineNumber} is empty");
            }

            var value = ParseFields(fields, lineNumber, fileData.FileName);
            values.Add(value);
        }

        if (values.Count < MinLineCount)
        {
            throw new CsvParseException("Invalid number of lines");
        }

        return values;
    }

    private static TextFieldParser CreateFieldParser(Stream stream)
    {
        return new TextFieldParser(stream)
        {
            TextFieldType = FieldType.Delimited,
            Delimiters = CsvDelimiters
        };
    }

    private Value ParseFields(string[] fields, long lineNumber, string fileName)
    {
        CultureInfo cultureInfo = _cultureProvider.CurrentCultureInfo;

        if (fields.Length is not FieldsCount
            || !DateTime.TryParseExact(fields[0], DateTimeFormat, cultureInfo, DateTimeStyles.None, out var dateTime)
            || !int.TryParse(fields[1], out var timeInSeconds)
            || !double.TryParse(fields[2], cultureInfo, out var rate))
        {
            throw new CsvParseException($"Incorrect data on line {lineNumber}");
        }

        var value = new Value
        {
            FileName = fileName,
            DateTime = dateTime,
            TimeInSeconds = timeInSeconds,
            Rate = rate
        };

        ValidationResult validationResult = _valueValidator.Validate(value);
        if (!validationResult.IsValid)
        {
            string validationErrorMessage = string.Join("", validationResult.Errors);
            string exceptionMessage = $"Line {lineNumber}: {validationErrorMessage}";
            throw new CsvParseException(exceptionMessage);
        }

        return value;
    }
}