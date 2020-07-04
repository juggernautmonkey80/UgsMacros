using Fractions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace UgsMacros.Macros.Cutting
{
  public static class StringExtensions
  {
    public static decimal? ToMillimeters(this string value, decimal nonZeroOffset = 0m)
    {
      if (string.IsNullOrWhiteSpace(value))
      {
        return null;
      }

      var parts = value.Split('&', ' ')
          .Select(v => Fraction.FromString(v))
          .ToList();

      var multiplier = parts[0].IsNegative ? -1m : 1m;

      var result = parts[0].ToDecimal();
      foreach (var part in parts.Skip(1))
      {
        result += multiplier * part.ToDecimal();
      }

      result = Math.Round(result * 25.4m, 3);

      if (result != 0m)
      {
        result += multiplier * nonZeroOffset;
      }

      return result;
    }
  }
}
