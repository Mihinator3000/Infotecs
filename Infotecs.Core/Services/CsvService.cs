using Infotecs.Abstractions.Core.Parsers;
using Infotecs.Abstractions.Core.Services;
using Infotecs.Abstractions.Core.Tools;
using Infotecs.Abstractions.DataAccess;
using Infotecs.Domain.Models;
using Infotecs.Domain.ValueTypes;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Core.Services;

public class CsvService : ICsvService
{
    private readonly ICsvParser _csvParser;
    private readonly IInfotecsDatabaseContext _context;
    private readonly ICalculator<ValuesData, Result> _resultCalculator;

    public CsvService(
        ICsvParser csvParser,
        IInfotecsDatabaseContext context,
        ICalculator<ValuesData, Result> resultCalculator)
    {
        _csvParser = csvParser;
        _context = context;
        _resultCalculator = resultCalculator;
    }

    public async Task Upload(FileData fileData)
    {
        IReadOnlyCollection<Value> values = _csvParser.Parse(fileData);

        await RemoveItemsByFileName(fileData.FileName);

        await _context.Values.AddRangeAsync(values);

        var valuesData = new ValuesData(values, fileData.FileName);
        Result result = _resultCalculator.Calculate(valuesData);
        await _context.Results.AddAsync(result);
        
        await _context.SaveChangesAsync();
    }

    private async Task RemoveItemsByFileName(string fileName)
    {
        Result? result = await _context.Results.FirstOrDefaultAsync(r => r.FileName == fileName);

        if (result is null)
        {
            return;
        }

        _context.Results.Remove(result);

        IQueryable<Value> values = _context.Values.Where(v => v.FileName == fileName);
        _context.Values.RemoveRange(values);
    }
}