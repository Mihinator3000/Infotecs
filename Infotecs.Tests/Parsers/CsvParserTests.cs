using AutoFixture;
using Infotecs.Abstractions.Core.Providers;
using Infotecs.Tests.Tools;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using Infotecs.Core.Parsers;
using Infotecs.Core.Providers;
using Infotecs.Domain.Exceptions;
using Infotecs.Domain.Models;
using Infotecs.Domain.ValueTypes;
using Infotecs.Tests.Extensions;
using Xunit;

namespace Infotecs.Tests.Parsers;

public class CsvParserTests
{
    private static readonly DateTime CurrentDateTimeMock = new(2023, 1, 1);
    private static readonly ICultureProvider CultureProvider = new RuCultureProvider();

    private readonly Fixture _fixture;
    private readonly ValueGenerator _valueGenerator;
    private readonly CsvTools _csvTools;
    private readonly IValidator<Value> _passValueValidator;

    public CsvParserTests()
    {
        _fixture = new Fixture();

        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider
            .Setup(x => x.CurrentDateTime)
            .Returns(CurrentDateTimeMock);

        _valueGenerator = new ValueGenerator(dateTimeProvider.Object);
        _csvTools = new CsvTools(CultureProvider.CurrentCultureInfo);

        var valueValidator = new Mock<IValidator<Value>>();
        valueValidator
            .Setup(x => x.Validate(It.IsAny<Value>()))
            .Returns(new ValidationResult());
        _passValueValidator = valueValidator.Object;
    }
    
    [Theory]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(1000)]
    public void ValidFileData_ReturnedValueCollection(int lineCount)
    {
        string fileName = _fixture.Create<string>();

        IReadOnlyList<Value> values = _valueGenerator.GenerateCollection(lineCount).ToArray();

        var fileData = new FileData(fileName, _csvTools.CreateCsvStream(values));

        var csvParser = new CsvParser(CultureProvider, _passValueValidator);

        IReadOnlyList<Value> valuesResult = csvParser.Parse(fileData).ToArray();

        Assert.Equal(lineCount, values.Count);

        for (int i = 0; i < lineCount; i++)
        {
            var value = values[i];
            var valueResult = valuesResult[i];
            Assert.Equal(fileName, valueResult.FileName);
            Assert.Equal(value.DateTime.Ticks, valueResult.DateTime.Ticks);
            Assert.Equal(value.TimeInSeconds, valueResult.TimeInSeconds);
            Assert.Equal(value.Rate, valueResult.Rate);
        }
    }

    [Fact]
    public void InvalidData_ThrowCsvParseException()
    {
        string fileName = _fixture.Create<string>();

        const string invalidFileContent = """
            some invalid stuff
            not csv either
            """;

        var fileData = new FileData(fileName, invalidFileContent.ToStream());

        var csvParser = new CsvParser(CultureProvider, _passValueValidator);

        Assert.Throws<CsvParseException>(() => csvParser.Parse(fileData));
    }

    [Fact]
    public void EmptyFileData_ThrowCsvParseException()
    {
        string fileName = _fixture.Create<string>();

        const string invalidFileContent = "";

        var fileData = new FileData(fileName, invalidFileContent.ToStream());

        var csvParser = new CsvParser(CultureProvider, _passValueValidator);

        Assert.Throws<CsvParseException>(() => csvParser.Parse(fileData));
    }

    [Fact]
    public void LinesCountIsMoreThan_10000_ThrowCsvParseException()
    {
        const int linesCount = 10001;
        string fileName = _fixture.Create<string>();

        IReadOnlyList<Value> values = _valueGenerator.GenerateCollection(linesCount).ToArray();

        var fileData = new FileData(fileName, _csvTools.CreateCsvStream(values));

        var csvParser = new CsvParser(CultureProvider, _passValueValidator);

        Assert.Throws<CsvParseException>(() => csvParser.Parse(fileData));
    }

    [Fact]
    public void IncorrectNumberOfArgumentInLine_ThrowCsvParserException()
    {
        string fileName = _fixture.Create<string>();

        const string invalidFileContent = """
            2022-03-18_09-18-17;1744;1632,472
            2022-04-18_09-18-14;5345
            """;

        var fileData = new FileData(fileName, invalidFileContent.ToStream());

        var csvParser = new CsvParser(CultureProvider, _passValueValidator);

        Assert.Throws<CsvParseException>(() => csvParser.Parse(fileData));
    }

    [Fact]
    public void ValueValidatorFailed_ThrowCsvParserException()
    {
        var valueValidatorMock = new Mock<IValidator<Value>>();
        valueValidatorMock
            .Setup(x => x.Validate(It.IsAny<Value>()))
            .Returns(new ValidationResult(new ValidationFailure[] {new(nameof(DateTime), "DateTime is incorrect")}));
        var failedValueValidator = valueValidatorMock.Object;

        const int linesCount = 100;
        string fileName = _fixture.Create<string>();

        IReadOnlyList<Value> values = _valueGenerator.GenerateCollection(linesCount).ToArray();

        var fileData = new FileData(fileName, _csvTools.CreateCsvStream(values));

        var csvParser = new CsvParser(CultureProvider, failedValueValidator);

        Assert.Throws<CsvParseException>(() => csvParser.Parse(fileData));
    }
}