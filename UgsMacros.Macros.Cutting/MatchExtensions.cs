using Fractions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace UgsMacros.Macros.Cutting
{
  public static class MatchExtensions
  {
    public static decimal? GetMillimeters(this Match match, string name, decimal nonZeroOffset = 0m)
    {
      return match.Groups[name].Value.ToMillimeters(nonZeroOffset);
    }
  }
}
