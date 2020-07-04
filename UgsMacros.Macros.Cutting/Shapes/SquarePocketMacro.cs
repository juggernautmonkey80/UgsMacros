using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Mortise
{
    //[Macro("square-pocket")]
    public class SquarePocketMacro : IHelpfulMacro
    {
        private IMacroVariableSet _variables;

        private RegexDecimal _width;
        private RegexDecimal _height;
        private RegexDecimal _depth;

        public SquarePocketMacro(
            IMacroVariableSet variables)
        {
            _variables = variables;

            _width = new RegexDecimal("width");
            _height = new RegexDecimal("height");
            _depth = new RegexDecimal("depth");
        }

        public string MatchString => $@"^square-pocket\s+{_width.Expression}x{_height.Expression}\s+{_depth.Expression}";

        public bool Execute(ICommandSender commandSender, Match match)
        {
            var bitWidth = _variables.BitWidth();
            if (bitWidth.HasValue)
            {
                var width = _width.GetMillimeters(match);
                var height = _height.GetMillimeters(match);
                var depth = _depth.GetMillimeters(match);

                CutPocket(commandSender, width, height, depth, bitWidth.Value);
            }
            else
            {
                Console.WriteLine("!! Bit Width Not Set.");
            }

            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("From a center point, cuts a pocket with the given width (Y-axis) and a height (X-axis) to the given depth");
            if (helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine(" Pocket will be rounded depending on the bit size");
                Console.WriteLine();
                Console.WriteLine("Syntax:");
                Console.WriteLine(" square-pocket (WIDTH)x(HEIGHT) (DEPTH)");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine(" square-pocket 0.5x1&1/4 0.128 - cuts a pocket a half inch wide, 1.25 inches long and 1/8th deep");
            }
        }

        private void CutPocket(ICommandSender commandSender, decimal width, decimal height, decimal depth, decimal bitWidth)
        {
            var cutHeight = height - bitWidth;
            var upperX = decimal.Round(cutHeight / 2, 3);
            var lowerX = cutHeight - upperX;

            var cutWidth = width - bitWidth;
            var leftY = decimal.Round(cutWidth / 2, 3);
            var rightY = cutWidth - leftY;

            commandSender.Init();

            // Rough pass

            // Clear out

            // Finish pass
            
            /* from center only */
            commandSender.SendLabeledCommand("Position", $"X{-1 * upperX}", init: false);
            commandSender.SendLabeledCommand("Plunge", $"Z{-1 * depth}", init: false);
            /* from center only */

            commandSender.SendLabeledCommand("Finishing 1/2 Left", $"Y{ -1 * leftY}", init: false);
            commandSender.SendLabeledCommand("Finishing Down", $"X{cutHeight}", init: false);
            commandSender.SendLabeledCommand("Finishing Right", $"Y{cutWidth}", init: false);
            commandSender.SendLabeledCommand("Finishing Up", $"X{-1 * cutHeight}", init: false);
            commandSender.SendLabeledCommand("Finishing 1/2 Left", $"Y{ -1 * rightY}", init: false);
        }
    }
}
