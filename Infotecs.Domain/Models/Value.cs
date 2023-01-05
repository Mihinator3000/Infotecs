namespace Infotecs.Domain.Models;

public class Value
{
    public int Id { get; protected init; }
    public required string FileName { get; init; }
    public required DateTime DateTime { get; init; }
    public required int TimeInSeconds { get; init; }
    public required double Rate { get; init; }
}