using Infotecs.Domain.Models;

namespace Infotecs.Domain.ValueTypes;

public readonly record struct ValuesData(
    IReadOnlyCollection<Value> Values,
    string FileName);