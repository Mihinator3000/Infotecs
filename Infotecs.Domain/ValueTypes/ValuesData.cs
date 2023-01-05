using Infotecs.Domain.Models;

namespace Infotecs.Domain.ValueTypes;

public record struct ValuesData(IReadOnlyCollection<Value> Values, string FileName);