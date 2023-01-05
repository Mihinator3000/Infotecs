using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infotecs.DataAccess.Converters;

public class TimeSpanConverter : ValueConverter<TimeSpan, long>
{
    public TimeSpanConverter()
        : base(s => s.Ticks, l => TimeSpan.FromTicks(l))
    {
    }
}