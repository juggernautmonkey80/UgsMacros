using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Cutting
{
    [Macro("@")]
    public class AtDecimalXYZ : IHelpfulMacro
    {
        private readonly IMacroVariableSet _variables;

        public AtDecimalXYZ(
            IMacroVariableSet variables)
        {
            _variables = variables;
        }

        public string MatchString => $@"^@(?<x>[0-9 &\-\./]+),(?<y>[0-9 \-\./]+)(,(?<z>[0-9 \-\./]+))*(?<bit>\s+\+bit)*$";

        public string BuildGCode(Match match)
        {
            var nonZeroOffset = 0m;
            if (!string.IsNullOrWhiteSpace(match.Groups["bit"].Value))
            {
                nonZeroOffset = _variables.GetBitWidth();
            }

            var x = match.GetMillimeters("x", nonZeroOffset).Value;
            var y = match.GetMillimeters("y", nonZeroOffset).Value;
            var z = match.GetMillimeters("z") ?? 0m;

            return $"X{x} Y{y} Z{z}";
        }

        public bool Execute(ICommandSender commandSender, Match match)
        {
            var gcode = BuildGCode(match);

            commandSender.SendLabeledCommand("Cut", gcode);

            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("Moves the bit with line interpolation by a relative X & Y (with optional Z) at the standard feed rate");
            if (helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine(" @2.5,0 - Moves the bit 2.5 inches in the postitive x.");
                Console.WriteLine(" @2&1/2,0 - Moves the bit 2.5 inches in the postitive x.");
                Console.WriteLine(" @0,0,-0.24 - Plunges down 1/4 inch.");
            }
        }
    }
}
