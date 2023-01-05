using Infotecs.Domain.Models;
using Infotecs.Domain.ValueTypes;

namespace Infotecs.Abstractions.Core.Parsers;

public interface ICsvParser
{
    IReadOnlyCollection<Value> Parse(FileData fileData);
}