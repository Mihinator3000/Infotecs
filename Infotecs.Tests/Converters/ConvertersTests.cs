using AutoFixture;
using AutoMapper;
using Infotecs.Domain.Models;
using Infotecs.Dto.Extensions;
using Infotecs.Dto.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Infotecs.Tests.Converters;

public class ConvertersTests
{
    private readonly Fixture _fixture;
    private readonly IMapper _mapper;

    public ConvertersTests()
    {
        _fixture = new Fixture();

        var serviceProvider = new ServiceCollection()
            .AddMappingConfiguration()
            .BuildServiceProvider();

        _mapper = serviceProvider.GetRequiredService<IMapper>();
    }

    [Fact]
    public void Convert_Value_ToDto_AllIsMapped()
    {
        var value = _fixture.Create<Value>();

        var valueDto = new ValueDto(
            value.DateTime,
            value.TimeInSeconds,
            value.Rate);

        var mappedValueDto = _mapper.Map<ValueDto>(value);

        Assert.Equal(valueDto, mappedValueDto);
    }

    [Fact]
    public void Convert_Result_ToDto_AllIsMapped()
    {
        var result = _fixture.Create<Result>();

        var resultDto = new ResultDto(
            result.FileName,
            result.AllTime,
            result.MinTime,
            result.AverageTimeInSeconds,
            result.AverageRate,
            result.MedianRate,
            result.MaxRate,
            result.MinRate,
            result.LinesCount);

        var mappedResultDto = _mapper.Map<ResultDto>(result);

        Assert.Equal(resultDto, mappedResultDto);
    }
}