using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Cutting
{
    [Macro("@-with-z")]
    public class AtDecimalXYZ : IMacro
    {
        private RegexDecimal _x;
        private RegexDecimal _y;
        private RegexDecimal _z;

        public AtDecimalXYZ()
        {
            _x = new RegexDecimal("x");
            _y = new RegexDecimal("y");
            _z = new RegexDecimal("z");
        }

        public string MatchString => $"^@{_x.Expression},{_y.Expression},{_z.Expression}$";

        public bool Execute(ICommandSender restClient, Match match, Func<string, bool?> translator)
        {
            var x = _x.GetMillimeters(match);
            var y = _y.GetMillimeters(match);
            var z = _z.GetMillimeters(match);

            restClient.SendLabeledCommand("Cut", $"X{x} Y{y} Z{z}");

            return true;
        }
    }
}
