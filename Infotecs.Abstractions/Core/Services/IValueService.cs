using Infotecs.Dto.Models;

namespace Infotecs.Abstractions.Core.Services;

public interface IValueService
{
    Task<IReadOnlyCollection<ValueDto>> Get(string fileName);
}