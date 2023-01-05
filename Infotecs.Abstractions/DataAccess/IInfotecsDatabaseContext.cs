using Infotecs.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Abstractions.DataAccess;

public interface IInfotecsDatabaseContext
{
    DbSet<Value> Values { get; }
    DbSet<Result> Results { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}