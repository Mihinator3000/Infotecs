using Infotecs.Domain.Exceptions;

namespace Infotecs.WebApi.Extensions;

public static class StringExtensions
{
    private const string CsvExtension = ".csv";

    public static string GetFileNameWithoutCsvExtension(this string fileName)
    {
        if (Path.GetExtension(fileName) is not CsvExtension)
            throw new CsvParseException($"Provided file was not in \"{CsvExtension}\" format");

        return Path.GetFileNameWithoutExtension(fileName);
    }
}