using AutoFixture;
using FluentAssertions;
using Infotecs.Abstractions.Core.Providers;
using Infotecs.Abstractions.Core.Tools;
using Infotecs.Core.Tools;
using Infotecs.Domain.Models;
using Infotecs.Domain.ValueTypes;
using Infotecs.Tests.Tools;
using Moq;
using Xunit;

namespace Infotecs.Tests.Services;

public class ResultCalculatorTests
{
    private static readonly DateTime CurrentDateTimeMock = new(2030, 7, 10);

    private readonly Fixture _fixture;
    private readonly ValueGenerator _valueGenerator;
    private readonly ICalculator<ValuesData, Result> _calculator;

    public ResultCalculatorTests()
    {
        _fixture = new Fixture();

        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider
            .Setup(x => x.CurrentDateTime)
            .Returns(CurrentDateTimeMock);

        _valueGenerator = new ValueGenerator(dateTimeProvider.Object);
        _calculator = new ResultCalculator();
    }

    [Fact]
    public void CalculateResults_MedianRate_IsCorrect()
    {
        string fileName = _fixture.Create<string>();

        var values = new[]
        {
            new Value
            {
                FileName = fileName,
                DateTime = _fixture.Create<DateTime>(),
                Rate = 0,
                TimeInSeconds = _fixture.Create<int>()
            },
            new Value
            {
                FileName = fileName,
                DateTime = _fixture.Create<DateTime>(),
                Rate = 1,
                TimeInSeconds = _fixture.Create<int>()
            }
        };

        const double medianRate = 0.5;

        Result result = _calculator.Calculate(new ValuesData(values, fileName));

        result.MedianRate.Should().Be(medianRate);
    }

    [Fact]
    public void CalculateResults_MedianRateOfOddNumbersCollection_IsCorrect()
    {
        string fileName = _fixture.Create<string>();

        const double medianRate = 214.5;

        var values = new[]
        {
            new Value
            {
                FileName = fileName,
                DateTime = _fixture.Create<DateTime>(),
                Rate = 4324.543,
                TimeInSeconds = _fixture.Create<int>()
            },
            new Value
            {
                FileName = fileName,
                DateTime = _fixture.Create<DateTime>(),
                Rate = medianRate,
                TimeInSeconds = _fixture.Create<int>()
            },
            new Value
            {
                FileName = fileName,
                DateTime = _fixture.Create<DateTime>(),
                Rate = 6,
                TimeInSeconds = _fixture.Create<int>()
            },
        };

        Result result = _calculator.Calculate(new ValuesData(values, fileName));

        result.MedianRate.Should().Be(medianRate);
    }

    [Fact]
    public void CollectionIsEmpty_ThrowArgumentException()
    {
        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> emptyCollection = Array.Empty<Value>();

        var calculateFunction = () => _calculator.Calculate(new ValuesData(emptyCollection, fileName));
        calculateFunction.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CalculateResults_FileName_IsCorrect()
    {
        const int linesCount = 10;

        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> values = _valueGenerator.GenerateCollection(linesCount);

        Result result = _calculator.Calculate(new ValuesData(values, fileName));

        result.FileName.Should().Be(fileName);
    }

    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public void CalculateResults_MinTime_IsCorrect(int linesCount)
    {
        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> values = _valueGenerator.GenerateCollection(linesCount);

        Result result = _calculator.Calculate(new ValuesData(values, fileName));
        DateTime minTime = values.Min(v => v.DateTime);

        result.MinTime.Should().Be(minTime);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(10000)]
    public void CalculateResults_AllTime_IsCorrect(int linesCount)
    {
        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> values = _valueGenerator.GenerateCollection(linesCount);

        Result result = _calculator.Calculate(new ValuesData(values, fileName));
        TimeSpan allTime = values.Max(v => v.DateTime) - values.Min(v => v.DateTime);

        result.AllTime.Should().Be(allTime);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    public void CalculateResults_AverageTime_IsCorrect(int linesCount)
    {
        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> values = _valueGenerator.GenerateCollection(linesCount);

        Result result = _calculator.Calculate(new ValuesData(values, fileName));
        double averageTimeInSeconds = values.Average(v => v.TimeInSeconds);

        result.AverageTimeInSeconds.Should().Be(averageTimeInSeconds);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    public void CalculateResults_AverageRate_IsCorrect(int linesCount)
    {
        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> values = _valueGenerator.GenerateCollection(linesCount);

        Result result = _calculator.Calculate(new ValuesData(values, fileName));
        double averageRate = values.Average(v => v.Rate);

        result.AverageRate.Should().Be(averageRate);
    }

    [Theory]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(5000)]
    public void CalculateResults_Rates_AreCorrect(int linesCount)
    {
        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> values = _valueGenerator.GenerateCollection(linesCount);

        Result result = _calculator.Calculate(new ValuesData(values, fileName));
        double minRate = values.Min(v => v.Rate);
        double maxRate = values.Max(v => v.Rate);

        result.MinRate.Should().Be(minRate);
        result.MaxRate.Should().Be(maxRate);
    }

    [Theory]
    [InlineData(32)]
    [InlineData(640)]
    [InlineData(4324)]
    public void CalculateResults_LinesCount_IsCorrect(int linesCount)
    {
        string fileName = _fixture.Create<string>();
        IReadOnlyCollection<Value> values = _valueGenerator.GenerateCollection(linesCount);

        Result result = _calculator.Calculate(new ValuesData(values, fileName));

        result.LinesCount.Should().Be(linesCount);
    }
}