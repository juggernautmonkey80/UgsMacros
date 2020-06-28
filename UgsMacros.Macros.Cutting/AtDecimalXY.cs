﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Cutting
{
    [Macro("@")]
    public class AtDecimalXY : IHelpfulMacro
    {
        private RegexDecimal _x;
        private RegexDecimal _y;

        public AtDecimalXY()
        {
            _x = new RegexDecimal("x");
            _y = new RegexDecimal("y");
        }

        public string MatchString => $"^@{_x.Expression},{_y.Expression}$";

        public bool Execute(ICommandSender restClient, Match match, Func<string, bool?> translator)
        {
            var x = _x.GetMillimeters(match);
            var y = _y.GetMillimeters(match);

            restClient.SendLabeledCommand("Cut", $"X{x} Y{y}");

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
