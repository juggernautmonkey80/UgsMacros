using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Mortise
{
    //[Macro("miter")]
    public class MiterMacro : IHelpfulMacro
    {
        private IMacroVariableSet _variables;

        private RegexDecimal _width;
        private RegexDecimal _depth;

        public MiterMacro(
            IMacroVariableSet variables)
        {
            _variables = variables;

            _width = new RegexDecimal("width");
            _depth = new RegexDecimal("depth");
        }

        public string MatchString => $@"^miter\s+(?<type>-x|x)\s+{_width.Expression}\s+{_depth.Expression}$";

        public bool Execute(ICommandSender commandSender, Match match)
        {
            const decimal step = 3.175m;

            int xFactor = 1;
            switch (match.Groups["type"].Value.ToLower())
            {
                case "x": xFactor = 1; break;
                case "-x": xFactor = -1; break;
            }

            var width = _width.GetMillimeters(match);
            var depth = _depth.GetMillimeters(match);

            var depths = new List<decimal>();
            while (depth > step)
            {
                depths.Add(step);
                depth -= step;
            }

            depths.Add(depth);

            var feed = _variables.FeedRate();

            commandSender.Init();
            foreach (var stepDepth in depths)
            {
                commandSender.SendLabeledCommand("Plunge", $"X{stepDepth * xFactor} Z{-1 * stepDepth} F{feed}", init: false);
                commandSender.SendLabeledCommand("Cut", $"Y{width} F{feed}", init: false);
                commandSender.SendLabeledCommand("Return Cut", $"Y{-1 * width} F{feed * 3}", init: false);
            }

            var totalPlunge = depths.Sum();
            commandSender.SendLabeledCommand("Return", $"X{-1 * totalPlunge * xFactor} Z{totalPlunge} F{feed * 3}", init: false);

            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("With a 90deg v-bit from a point on the left side, cuts a miter with a width (Y-axis) and a depth (Z-axis)");
            if (helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine();
                Console.WriteLine("Syntax:");
                Console.WriteLine(" miter x (WIDTH) (DEPTH)");
                Console.WriteLine(" miter -x (WIDTH) (DEPTH)");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine(" miter x 3 0.5 - cuts a miter on a 0.5 inch board in the positive x direction");
            }
        }
    }
}
