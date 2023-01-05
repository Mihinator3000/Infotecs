using AutoFixture;
using FluentAssertions;
using Infotecs.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Infotecs.Tests.DataAccess;

public class ValueTests : DataAccessTestBase
{
    private readonly Fixture _fixture;

    public ValueTests()
    {
        _fixture = new Fixture();
    }

    [Fact]
    public async Task SaveChanges_NoExceptionThrown()
    {
        var value = _fixture.Create<Value>();

        await Context.Values.AddAsync(value);

        Func<Task<int>> saveFunction = () => Context.SaveChangesAsync();

        await saveFunction.Should().NotThrowAsync();
    }

    [Fact]
    public async Task SaveChanges_EntityFetched()
    {
        var value = _fixture.Create<Value>();

        await Context.Values.AddAsync(value);
        await Context.SaveChangesAsync();

        Func<Task<Value?>> fetchFunction = async () => await Context.Values.FirstOrDefaultAsync(r => r.FileName == value.FileName);

        var fetchedResult = await fetchFunction.Should().NotThrowAsync();
        fetchedResult.Subject.Should().NotBeNull();
    }
}