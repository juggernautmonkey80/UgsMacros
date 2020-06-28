using System;

namespace UgsMacros.Framework
{
    public static class StandardVariableExtensions
    {
        public static decimal? BitWidth(this IMacroVariableSet variables, decimal? width= null)
        {
            return variables.Value<decimal>("standard-bit-width", width);
        }

        public static int FeedRate(this IMacroVariableSet variables, int? feed = null)
        {
            var result = variables.Value<int>("cut-feed-rate", feed);
            return result.GetValueOrDefault(300);
        }

        public static T? Value<T>(this IMacroVariableSet variables, string key, T? value = null)
            where T : struct
        {
            if (value.HasValue)
            {
                variables[key] = value.Value;
                Console.WriteLine($"  [{key}] set to {value.Value}");
            }

            if (variables.TryGetValue(key, out object currentValue))
            {
                return (T)currentValue;
            }

            return null;
        }
    }
}
