using Infotecs.Domain.ValueTypes;

namespace Infotecs.Abstractions.Core.Services;

public interface ICsvService
{
    Task Upload(FileData fileData);
}