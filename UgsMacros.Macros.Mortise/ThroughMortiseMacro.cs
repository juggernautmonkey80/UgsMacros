﻿using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Mortise
{
    //[Macro("through-mortise")]
    public class ThroughMortiseMacro : IHelpfulMacro
    {
        private IMacroVariableSet _variables;

        //private RegexDecimal _width;
        //private RegexDecimal _height;
        //private RegexDecimal _depth;

        public ThroughMortiseMacro(
            IMacroVariableSet variables)
        {
            _variables = variables;

            //_width = new RegexDecimal("width");
            //_height = new RegexDecimal("height");
            //_depth = new RegexDecimal("depth");
        }

        public string MatchString => $@"^through-mortise";

        public bool Execute(ICommandSender commandSender, Match match)
        {
            var bitWidth = _variables.BitWidth();
            if (bitWidth.HasValue)
            {
                //var width = _width.GetMillimeters(match);
                //var height = _height.GetMillimeters(match);
                //var depth = _depth.GetMillimeters(match);

                //CutMortise(commandSender, width, height, depth, bitWidth.Value);
            }
            else
            {
                Console.WriteLine("!! Bit Width Not Set.");
            }

            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("From a center point, cuts a mortis a little over half way through the stock");
            if (helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine(" The exact same through-mortise opperation can then be run on the other side");
                Console.WriteLine();
                Console.WriteLine("Syntax:");
                Console.WriteLine(" hinge-mortise (WIDTH)x(HEIGHT) (DEPTH)");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine(" hinge-mortise 0.5x1&1/4 0.128 - cuts a mortis a half inch wide, 1.25 inches long and 1/8th deep");
            }
        }

        private void CutMortise(ICommandSender commandSender, decimal width, decimal height, decimal depth, decimal bitWidth)
        {
            var cutHeight = height - bitWidth;
            var positioningX = -1 * decimal.Round(cutHeight / 2, 3);
            var returnX = -1 * (cutHeight + positioningX);

            commandSender.Init();
            commandSender.SendLabeledCommand("Position", $"X{positioningX}", init: false);
            commandSender.SendLabeledCommand("Plunge", $"Z{-1 * depth}", init: false);
            commandSender.SendLabeledCommand("Enter", $"Y{width}", init: false);
            commandSender.SendLabeledCommand("Cut", $"X{cutHeight}", init: false);
            commandSender.SendLabeledCommand("Exit", $"Y{-1 * width}", init: false);
            commandSender.SendLabeledCommand("Retract", $"Z{depth}", init: false);
            commandSender.SendLabeledCommand("Return", $"X{returnX}", init: false);
        }
    }
}
