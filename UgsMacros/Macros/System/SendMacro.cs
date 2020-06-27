using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.System
{
    public class SendMacro : IMacro
    {
        public string MatchString => ">(?<gcode>.+)";

        public bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator)
        {
            commandSender.SendCommand(match.Groups["gcode"].Value);
            return true;
        }
    }
}
