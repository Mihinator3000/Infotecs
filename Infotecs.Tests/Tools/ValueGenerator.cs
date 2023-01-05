using Infotecs.Abstractions.Core.Providers;
using Infotecs.Domain.Models;
using Infotecs.Tests.Extensions;

namespace Infotecs.Tests.Tools;

public class ValueGenerator
{
    private const int MinTimeInSeconds = 0;
    private const double MinRate = 0;

    private static readonly long MinDateTimeTotalSeconds = new DateTime(2000, 1, 1).GetTotalSeconds();

    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly Random _random;

    public ValueGenerator(IDateTimeProvider dateTimeProvider, Random? random = null)
    {
        _dateTimeProvider = dateTimeProvider;
        _random = random ?? new Random();
    }

    public Value Generate(string fileName = "", DateTime? dateTime = null, int? timeInSeconds = null, double? rate = null)
    {
        return new Value
        {
            FileName = fileName,
            DateTime = dateTime ?? GenerateValidDateTime(),
            TimeInSeconds = timeInSeconds ?? GenerateValidTimeInSeconds(),
            Rate = rate ?? GenerateValidRate()
        };
    }

    public IReadOnlyCollection<Value> GenerateCollection(int lineCount, string fileName = "")
    {
        return Enumerable
            .Range(1, lineCount)
            .Select(x => Generate(fileName))
            .ToArray();
    }

    private DateTime GenerateValidDateTime()
    {
        long dateTimeNowTotalSeconds = _dateTimeProvider.CurrentDateTime.GetTotalSeconds();
        long dateTimeTotalSeconds = _random.NextInt64(MinDateTimeTotalSeconds, dateTimeNowTotalSeconds);
        return new DateTime(dateTimeTotalSeconds * DateTimeExtensions.TicksInSecond);
    }

    private int GenerateValidTimeInSeconds()
        => _random.Next(MinTimeInSeconds, int.MaxValue);

    private double GenerateValidRate()
        => _random.NextDouble() * (double.MaxValue - MinRate) + MinRate;
}