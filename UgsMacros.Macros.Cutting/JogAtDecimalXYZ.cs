using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Cutting
{
    [Macro("@@")]
    public class JogAtDecimalXYZ : IHelpfulMacro
    {
        private AtDecimalXYZ _cutter;

        public JogAtDecimalXYZ(
            AtDecimalXYZ cutter)
        {
            _cutter = cutter;
        }

        public string MatchString => _cutter.MatchString.Replace("@", "@@");

        public bool Execute(ICommandSender commandSender, Match match)
        {
            var gcode = _cutter.BuildGCode(match);

            commandSender.SendLabeledCommand("Jog", $"G21 G91 G00 {gcode}", init: false);

            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("Jogs the bit with line interpolation by a relative X & Y (with optional Z)");
            if (helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine(" @@2.5,0 - Quickly moves the bit 2.5 inches in the postitive x.");
                Console.WriteLine(" @@2&1/2,0 - Quickly moves the bit 2.5 inches in the postitive x.");
                Console.WriteLine(" @@0,0,-0.24 - Quickly plunges down 1/4 inch.");
            }
        }
    }
}
