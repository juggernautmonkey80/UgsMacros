using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Cutting
{
    [Macro("Set-Bit")]
    public class SetBitMacro : IHelpfulMacro
    {
        private RegexDecimal _width;
        private readonly IMacroVariableSet _variables;

        public SetBitMacro(
            IMacroVariableSet variables)
        {
            _variables = variables;
            _width = new RegexDecimal("width");
        }

        public string MatchString => $@"^set-bit\s+{_width.Expression}$";

        public bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator)
        {
            var width = _width.GetMillimeters(match);
            _variables.BitWidth(width);
            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("Sets the standard bit width variable for macros that need to know the bit width");
            if (helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine("Examples:");
                Console.WriteLine(" set-bit 0.25 - sets the bit width to 1/4 inch.");
                Console.WriteLine(" set-bit 0&1/4 - sets teh bit width to 1/4 inch.");
            }
        }
    }
}
