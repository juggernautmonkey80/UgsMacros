using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Cutting
{
    [Macro("@X,Y")]
    public class AtDecimalXY : IMacro
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

            restClient.SendCommand($"X{x} Y{y}");

            return true;
        }
    }
}
