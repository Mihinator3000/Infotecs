namespace Infotecs.Abstractions.Core.Providers;

public interface IDateTimeProvider
{
    DateTime CurrentDateTime { get; }
}