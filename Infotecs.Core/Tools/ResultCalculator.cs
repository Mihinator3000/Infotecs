using Infotecs.Abstractions.Core.Tools;
using Infotecs.Domain.Models;
using Infotecs.Domain.ValueTypes;

namespace Infotecs.Core.Tools;

public class ResultCalculator : ICalculator<ValuesData, Result>
{
    public Result Calculate(ValuesData valuesData)
    {
        IReadOnlyCollection<Value> values = valuesData.Values;

        DateTime minTime = values.Min(v => v.DateTime);
        TimeSpan allTime = values.Max(v => v.DateTime) - minTime;

        return new Result
        {
            FileName = valuesData.FileName,
            MinTime = minTime,
            AllTime = allTime,
            AverageTimeInSeconds = values.Average(v => v.TimeInSeconds),
            AverageRate = values.Average(v => v.Rate),
            MedianRate = CalculateMedianRate(values),
            MinRate = values.Min(v => v.Rate),
            MaxRate = values.Max(v => v.Rate),
            LinesCount = values.Count
        };
    }

    private static double CalculateMedianRate(IEnumerable<Value> values)
    {
        double[] rates = values
            .Select(v => v.Rate)
            .Order()
            .ToArray();

        int middle = rates.Length / 2;

        if (rates.Length % 2 is 1)
        {
            return rates[middle];
        }
        
        return (rates[middle - 1] + rates[middle]) / 2;
    }
}