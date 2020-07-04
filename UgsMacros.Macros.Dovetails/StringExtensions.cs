using CommandLine;
using System.Linq;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.Dovetails
{
  public static class StringExtensions
  {
    public static T ParseAsCommandLine<T>(this string args)
    {
      var argArray = Regex
          .Split(args, @"-(\S+)")
          .Where(s => !string.IsNullOrWhiteSpace(s))
          .Select(s => s.StartsWith(" ") ? s : $"-{s}")
          .Select(s => s.Trim())
          .ToArray();

      var result = default(T);
      Parser.Default
          .ParseArguments<T>(argArray)
          .WithParsed<T>(
              options =>
              {
                result = options;
              })
          .WithNotParsed(
              err =>
              {
                throw new MacroFailedException("Macro not executed");
              });

      return result;
    }
  }
}
