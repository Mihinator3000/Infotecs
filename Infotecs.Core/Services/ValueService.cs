using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infotecs.Abstractions.Core.Services;
using Infotecs.Abstractions.DataAccess;
using Infotecs.Dto.Models;
using Microsoft.EntityFrameworkCore;

namespace Infotecs.Core.Services;

public class ValueService : IValueService
{
    private readonly IInfotecsDatabaseContext _context;
    private readonly IMapper _mapper;

    public ValueService(IInfotecsDatabaseContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyCollection<ValueDto>> Get(string fileName)
    {
        ValueDto[] values = await _context.Values
            .Where(v => v.FileName == fileName)
            .ProjectTo<ValueDto>(_mapper.ConfigurationProvider)
            .ToArrayAsync();

        return values;
    }
}