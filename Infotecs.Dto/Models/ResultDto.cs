namespace Infotecs.Dto.Models;

public record ResultDto(
    string FileName,
    TimeSpan AllTime,
    DateTime MinTime,
    double AverageTimeInSeconds,
    double AverageRate,
    double MedianRate,
    double MaxRate,
    double MinRate,
    int LinesCount);