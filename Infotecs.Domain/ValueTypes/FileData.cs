namespace Infotecs.Domain.ValueTypes;

public readonly record struct FileData(string FileName, Stream ReadStream);