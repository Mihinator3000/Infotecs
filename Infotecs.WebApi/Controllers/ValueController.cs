using Infotecs.Abstractions.Core.Services;
using Infotecs.Dto.Models;
using Infotecs.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ValueController : ControllerBase
{
    private readonly IValueService _service;

    public ValueController(IValueService service)
    {
        _service = service;
    }

    [HttpGet("get/by-full-filename/{fullFileName}")]
    public async Task<ActionResult<IReadOnlyCollection<ValueDto>>> GetByFullFileName(string fullFileName)
    {
        string fileName = fullFileName.GetFileNameWithoutCsvExtension();
        return await GetByFileName(fileName);
    }

    [HttpGet("get/by-filename/{fileName}")]
    public async Task<ActionResult<IReadOnlyCollection<ValueDto>>> GetByFileName(string fileName)
    {
        IReadOnlyCollection<ValueDto> values = await _service.Get(fileName);
        return Ok(values);
    }
}