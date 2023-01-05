namespace Infotecs.Domain.Models;

public class Result
{
    public int Id { get; protected init; }
    public required string FileName { get; init; }
    public required TimeSpan AllTime { get; init; }
    public required DateTime MinTime { get; init; }
    public required double AverageTimeInSeconds { get; init; }
    public required double AverageRate { get; init; }
    public required double MedianRate { get; init; }
    public required double MaxRate { get; init; }
    public required double MinRate { get; init; }
    public required int LinesCount { get; init; }
}