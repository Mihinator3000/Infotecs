using Infotecs.Abstractions.Core.Providers;

namespace Infotecs.Core.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime CurrentDateTime => DateTime.Now;
}