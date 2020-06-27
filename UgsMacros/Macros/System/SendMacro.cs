using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.System
{
    [Macro(">GCODE")]
    public class SendMacro : IHelpfulMacro
    {
        public string MatchString => ">(?<gcode>.+)";

        public bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator)
        {
            commandSender.SendCommand(match.Groups["gcode"].Value);
            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("Sends the provided gcode directly to UGS");
        }
    }
}
