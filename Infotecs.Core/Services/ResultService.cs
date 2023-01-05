using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infotecs.Abstractions.Core.Services;
using Infotecs.Abstractions.DataAccess;
using Infotecs.Domain.Exceptions;
using Infotecs.Domain.Models;
using Infotecs.Dto.Models;
using Infotecs.Dto.Ranges;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Core.Services;

public class ResultService : IResultService
{
    private readonly IInfotecsDatabaseContext _context;
    private readonly IMapper _mapper;

    public ResultService(IInfotecsDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<ResultDto>> GetAll()
    {
        ResultDto[] results = await _context.Results
            .ProjectTo<ResultDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync();

        return results;
    }

    public async Task<ResultDto> GetByFileName(string fileName)
    {
        Result? result = await _context.Results
            .FirstOrDefaultAsync(r => r.FileName == fileName);

        if (result is null)
        {
            throw new EntityNotFoundException($"No results by filename \"{fileName}.csv\"");
        }

        return _mapper.Map<ResultDto>(result);
    }

    public Task<IReadOnlyCollection<ResultDto>> FindByMinTimeRange(TimeRangeDto minTimeRange)
        => GetResultsByPredicate(r => minTimeRange.From <= r.MinTime && r.MinTime <= minTimeRange.To);

    public Task<IReadOnlyCollection<ResultDto>> FindByAverageRateRange(DoubleRangeDto rateRange)
        => GetResultsByPredicate(r => rateRange.From <= r.AverageRate && r.AverageRate <= rateRange.To);

    public Task<IReadOnlyCollection<ResultDto>> FindByAverageTimeInSecondsRange(DoubleRangeDto averageTimeInSecondsRange)
    {
        return GetResultsByPredicate(r =>
            averageTimeInSecondsRange.From <= r.AverageTimeInSeconds
            && r.AverageTimeInSeconds <= averageTimeInSecondsRange.To);
    }

    private async Task<IReadOnlyCollection<ResultDto>> GetResultsByPredicate(Expression<Func<Result, bool>> predicate)
    {
        ResultDto[] results = await _context.Results
            .Where(predicate)
            .ProjectTo<ResultDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync();

        return results;
    }
}