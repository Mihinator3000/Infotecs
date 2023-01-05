using Infotecs.Abstractions.Core.Services;
using Infotecs.Domain.Exceptions;
using Infotecs.Dto.Models;
using Infotecs.Dto.Ranges;
using Infotecs.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResultController : ControllerBase
{
    private readonly IResultService _service;

    public ResultController(IResultService service)
    {
        _service = service;
    }

    [HttpGet("get/all")]
    public async Task<ActionResult<IReadOnlyCollection<ResultDto>>> GetAll()
    {
        IReadOnlyCollection<ResultDto> values = await _service.GetAll();
        return Ok(values);
    }

    [HttpGet("get/by-full-filename/{fullFileName}")]
    public async Task<ActionResult<ResultDto>> GetByFullFileName(string fullFileName)
    {
        string fileName = fullFileName.GetFileNameWithoutCsvExtension();
        return await GetByFileName(fileName);
    }

    [HttpGet("get/by-filename/{fileName}")]
    public async Task<ActionResult<ResultDto>> GetByFileName(string fileName)
    {
        try
        {
            ResultDto value = await _service.GetByFileName(fileName);
            return Ok(value);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost("find/by-min-time-range")]
    public async Task<ActionResult<IReadOnlyCollection<ResultDto>>> FindByMinTimeRange(
        [FromBody]TimeRangeDto minTimeRange)
    {
        IReadOnlyCollection<ResultDto> values = await _service.FindByMinTimeRange(
            minTimeRange);

        return Ok(values);
    }

    [HttpPost("find/by-avg-rate-range")]
    public async Task<ActionResult<IReadOnlyCollection<ResultDto>>> FindByAverageRateRange(
        [FromBody]DoubleRangeDto averageRateRange)
    {
        IReadOnlyCollection<ResultDto> values = await _service.FindByAverageRateRange(
            averageRateRange);

        return Ok(values);
    }

    [HttpPost("find/by-avg-time-in-seconds")]
    public async Task<ActionResult<IReadOnlyCollection<ResultDto>>> FindByAverageTimeInSecondsRange(
        [FromBody]DoubleRangeDto averageTimeInSecondsRange)
    {
        IReadOnlyCollection<ResultDto> values = await _service.FindByAverageTimeInSecondsRange(
            averageTimeInSecondsRange);

        return Ok(values);
    }
}