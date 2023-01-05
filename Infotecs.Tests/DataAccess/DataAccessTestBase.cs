using Infotecs.DataAccess;
using Infotecs.Dto.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infotecs.Tests.DataAccess;

public abstract class DataAccessTestBase : IDisposable
{
    protected readonly InfotecsDatabaseContext Context;
    protected readonly IServiceProvider Provider;

    protected DataAccessTestBase()
    {
        var collection = new ServiceCollection();
        var id = Guid.NewGuid();

        collection.AddDbContext<InfotecsDatabaseContext>(o =>
            o.UseSqlite($"Data Source={id}.db"));

        collection.AddMappingConfiguration();
        Provider = collection.BuildServiceProvider();

        Context = Provider.GetRequiredService<InfotecsDatabaseContext>();
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}