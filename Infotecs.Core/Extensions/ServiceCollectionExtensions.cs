using FluentValidation;
using Infotecs.Abstractions.Core.Parsers;
using Infotecs.Abstractions.Core.Providers;
using Infotecs.Abstractions.Core.Services;
using Infotecs.Abstractions.Core.Tools;
using Infotecs.Core.Parsers;
using Infotecs.Core.Parsers.Validators;
using Infotecs.Core.Providers;
using Infotecs.Core.Services;
using Infotecs.Core.Tools;
using Infotecs.Domain.Models;
using Infotecs.Domain.ValueTypes;
using Microsoft.Extensions.DependencyInjection;

namespace Infotecs.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        return services
            .AddSingleton<IDateTimeProvider, DateTimeProvider>()
            .AddSingleton<ICultureProvider, RuCultureProvider>();
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICsvParser, CsvParser>()
            .AddScoped<IValidator<Value>, ValueValidator>()
            .AddScoped<ICalculator<ValuesData, Result>, ResultCalculator>()
            .AddScoped<ICsvService, CsvService>()
            .AddScoped<IValueService, ValueService>()
            .AddScoped<IResultService, ResultService>();
    }
}