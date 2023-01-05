using AutoFixture;
using FluentAssertions;
using Infotecs.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Xunit;

namespace Infotecs.Tests.DataAccess;

public class ResultTests : DataAccessTestBase
{
    private readonly Fixture _fixture;

    public ResultTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task SaveChanges_NoExceptionThrown()
    {
        var result = _fixture.Create<Result>();

        await Context.Results.AddAsync(result);

        Func<Task<int>> saveFunction = () => Context.SaveChangesAsync();

        await saveFunction.Should().NotThrowAsync();
    }

    [Fact]
    public async Task AddResultsWithSameFileName_ThrowDbUpdateException()
    {
        var result = _fixture.Create<Result>();
        var sameFileNameResult = _fixture.Create<Result>();
        sameFileNameResult.GetType().InvokeMember("FileName",
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
            Type.DefaultBinder, sameFileNameResult, new object? []{ result.FileName });
        
        await Context.Results.AddAsync(result);
        await Context.Results.AddAsync(sameFileNameResult);

        Func<Task<int>> saveFunction = () => Context.SaveChangesAsync();

        await saveFunction.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task SaveChanges_EntityFetched()
    {
        var result = _fixture.Create<Result>();
        
        await Context.Results.AddAsync(result);
        await Context.SaveChangesAsync();
        
        Func<Task<Result?>> fetchFunction = async () => await Context.Results.FirstOrDefaultAsync(r => r.FileName == result.FileName);

        var fetchedResult = await fetchFunction.Should().NotThrowAsync();
        fetchedResult.Subject.Should().NotBeNull();
    }
}