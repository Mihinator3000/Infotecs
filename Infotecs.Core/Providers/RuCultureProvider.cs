using System.Globalization;
using Infotecs.Abstractions.Core.Providers;

namespace Infotecs.Core.Providers;

public class RuCultureProvider : ICultureProvider
{
    public CultureInfo CurrentCultureInfo { get; } = new("ru-RU");
}