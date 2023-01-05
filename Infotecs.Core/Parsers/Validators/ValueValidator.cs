using FluentValidation;
using Infotecs.Abstractions.Core.Providers;
using Infotecs.Domain.Models;

namespace Infotecs.Core.Parsers.Validators;

public class ValueValidator : AbstractValidator<Value>
{
    private const int MinTimeInSeconds = 0;
    private const double MinRate = 0;

    private static readonly DateTime MinDateTime = new(2000, 1, 1);

    private readonly IDateTimeProvider _dateTimeProvider;

    public ValueValidator(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;

        RuleFor(v => v.DateTime)
            .Must(BeAValidDateTime)
            .WithMessage("DateTime is incorrect");

        RuleFor(v => v.TimeInSeconds)
            .GreaterThanOrEqualTo(MinTimeInSeconds)
            .WithMessage("Time in seconds is incorrect");

        RuleFor(v => v.Rate)
            .GreaterThanOrEqualTo(MinRate)
            .WithMessage("Rate is incorrect");
    }

    private bool BeAValidDateTime(DateTime dateTime)
        => MinDateTime < dateTime && dateTime < _dateTimeProvider.CurrentDateTime;
}