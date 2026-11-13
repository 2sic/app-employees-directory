using System.Linq;
using System.Text;

namespace AppCode.Extensions.VCard
{
  internal static class VCardStringExtensions
  {
    public static StringBuilder AppendLineIfValue(this StringBuilder builder, string prefix, string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
        builder.AppendLine($"{prefix}{value.Escape()}");

      return builder;
    }

    public static StringBuilder AppendLineIfRawValue(this StringBuilder builder, string prefix, string value)
    {
      if (!string.IsNullOrWhiteSpace(value))
        builder.AppendLine($"{prefix}{value}");

      return builder;
    }

    public static StringBuilder AppendLineIfAny(this StringBuilder builder, string line, params string[] values)
    {
      if (values.Any(value => !string.IsNullOrWhiteSpace(value)))
        builder.AppendLine(line);

      return builder;
    }

    public static string Escape(this string value)
      => (value ?? string.Empty)
        .Replace("\\", "\\\\")
        .Replace("\r\n", "\\n")
        .Replace("\n", "\\n")
        .Replace("\r", "\\n")
        .Replace(";", "\\;")
        .Replace(",", "\\,");
  }
}
