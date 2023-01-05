using Infotecs.Abstractions.DataAccess;
using Infotecs.DataAccess.Converters;
using Infotecs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.DataAccess;

public class InfotecsDatabaseContext : DbContext, IInfotecsDatabaseContext
{
    public InfotecsDatabaseContext(DbContextOptions<InfotecsDatabaseContext> options)
        : base(options)
    {
    }

    public required DbSet<Value> Values { get; init; }
    public required DbSet<Result> Results { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IAssemblyMarker).Assembly);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<TimeSpan>().HaveConversion<TimeSpanConverter>();
    }
}