using AutoFixture;
using FluentValidation.TestHelper;
using Infotecs.Abstractions.Core.Providers;
using Infotecs.Core.Parsers.Validators;
using Infotecs.Domain.Models;
using Moq;
using Xunit;

namespace Infotecs.Tests.Parsers;

public class ValueValidatorTests
{
    private static readonly DateTime CurrentDateTimeMock = new(2023, 1, 1);

    private readonly Fixture _fixture;
    private readonly ValueValidator _valueValidator;

    public ValueValidatorTests()
    {
        _fixture = new Fixture();

        var dateTimeProvider = new Mock<IDateTimeProvider>();
        dateTimeProvider
            .Setup(x => x.CurrentDateTime)
            .Returns(CurrentDateTimeMock);

        _valueValidator = new ValueValidator(dateTimeProvider.Object);
    }

    [Fact]
    public void ValueIsValid_ValidationPassed()
    {
        var validDateTime = new DateTime(2020, 2, 3);

        var validValue = new Value
        {
            FileName = _fixture.Create<string>(),
            DateTime = validDateTime,
            TimeInSeconds = 200,
            Rate = 2.0
        };
        
        var result = _valueValidator.TestValidate(validValue);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void InvalidTimeInSeconds_ValidationFailed()
    {
        const int invalidTimeInSeconds = -1;

        var validValue = new Value
        {
            FileName = _fixture.Create<string>(),
            DateTime = new DateTime(2020, 2, 3),
            TimeInSeconds = invalidTimeInSeconds,
            Rate = 3.0
        };

        var result = _valueValidator.TestValidate(validValue);

        result.ShouldHaveValidationErrorFor(x => x.TimeInSeconds);
    }

    [Fact]
    public void InvalidRate_ValidationFailed()
    {
        const double invalidRate = -5.5;

        var validValue = new Value
        {
            FileName = _fixture.Create<string>(),
            DateTime = new DateTime(2020, 2, 3),
            TimeInSeconds = 200,
            Rate = invalidRate
        };

        var result = _valueValidator.TestValidate(validValue);

        result.ShouldHaveValidationErrorFor(x => x.Rate);
    }

    [Fact]
    public void DateTimeIsLesserThan_01_01_2000_ValidationFailed()
    {
        var invalidDateTime = new DateTime(1999, 3, 5);

        var validValue = new Value
        {
            FileName = _fixture.Create<string>(),
            DateTime = invalidDateTime,
            TimeInSeconds = 300,
            Rate = 22
        };

        var result = _valueValidator.TestValidate(validValue);

        result.ShouldHaveValidationErrorFor(x => x.DateTime);
    }

    [Fact]
    public void DateTimeIsGreaterThan_CurrentDateTime_ValidationFailed()
    {
        var invalidDateTime = new DateTime(2024, 2, 1);

        var validValue = new Value
        {
            FileName = _fixture.Create<string>(),
            DateTime = invalidDateTime,
            TimeInSeconds = 23,
            Rate = 1.2
        };

        var result = _valueValidator.TestValidate(validValue);

        result.ShouldHaveValidationErrorFor(x => x.DateTime);
    }
}