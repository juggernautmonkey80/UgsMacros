using System;
using System.Collections.Generic;
using System.Text;

namespace UgsMacros.Framework
{
    public static class StandardVariableExtensions
    {
        public static decimal? BitWidth(this IMacroVariableSet variables, decimal? width= null)
        {
            return variables.Value<decimal>("standard-bit-width", width);
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
