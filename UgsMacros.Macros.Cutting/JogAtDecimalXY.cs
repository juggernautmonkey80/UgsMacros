using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Jogging
{
    [Macro("@@")]
    public class JogAtDecimalXY : IHelpfulMacro
    {
        private RegexDecimal _x;
        private RegexDecimal _y;

        public JogAtDecimalXY()
        {
            _x = new RegexDecimal("x");
            _y = new RegexDecimal("y");
        }

        public string MatchString => $"^@@{_x.Expression},{_y.Expression}$";

        public bool Execute(ICommandSender restClient, Match match, Func<string, bool?> translator)
        {
            var x = _x.GetMillimeters(match);
            var y = _y.GetMillimeters(match);

            restClient.SendLabeledCommand("Jog", $"G21 G91 G00 X{x} Y{y}", init: false);

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
