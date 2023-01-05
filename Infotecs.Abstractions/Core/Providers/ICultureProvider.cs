using System.Globalization;

namespace Infotecs.Abstractions.Core.Providers;

public interface ICultureProvider
{
    CultureInfo CurrentCultureInfo { get; }
}