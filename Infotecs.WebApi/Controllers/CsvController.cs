using Infotecs.Abstractions.Core.Services;
using Infotecs.Domain.Exceptions;
using Infotecs.Domain.ValueTypes;
using Infotecs.WebApi.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CsvController : ControllerBase
{
    private readonly ICsvService _service;

    public CsvController(ICsvService service)
    {
        _service = service;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        string filename = file.FileName.GetFileNameWithoutCsvExtension();
        await using Stream readStream = file.OpenReadStream();

        try
        {
            await _service.Upload(new FileData(filename, readStream));
            return Ok();
        }
        catch (CsvParseException e)
        {
            return UnprocessableEntity(e.Message);
        }
    }
}
