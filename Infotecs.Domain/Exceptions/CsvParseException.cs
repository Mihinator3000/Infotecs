namespace Infotecs.Domain.Exceptions;

public class CsvParseException : Exception
{
    public CsvParseException(string message)
        : base(message)
    {
    }
}