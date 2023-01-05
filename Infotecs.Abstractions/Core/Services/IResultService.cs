using Infotecs.Dto.Models;
using Infotecs.Dto.Ranges;

namespace Infotecs.Abstractions.Core.Services;

public interface IResultService
{
    Task<IReadOnlyCollection<ResultDto>> GetAll();

    Task<ResultDto> GetByFileName(string fileName);

    Task<IReadOnlyCollection<ResultDto>> FindByMinTimeRange(TimeRangeDto minTimeRange);

    Task<IReadOnlyCollection<ResultDto>> FindByAverageRateRange(DoubleRangeDto rateRange);

    Task<IReadOnlyCollection<ResultDto>> FindByAverageTimeInSecondsRange(DoubleRangeDto averageTimeInSecondsRange);
}