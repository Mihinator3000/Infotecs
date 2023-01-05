using System.Text;

namespace Infotecs.Tests.Extensions;

public static class StringExtensions
{
    public static Stream ToStream(this string s)
        => s.ToStream(Encoding.UTF8);

    public static Stream ToStream(this string s, Encoding encoding)
        => new MemoryStream(encoding.GetBytes(s ?? string.Empty));
}