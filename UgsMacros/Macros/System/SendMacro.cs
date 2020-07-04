using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.System
{
    [Macro(">")]
    public class SendMacro : IHelpfulMacro
    {
        public string MatchString => ">(?<gcode>.+)";

        public bool Execute(ICommandSender commandSender, Match match)
        {
            commandSender.SendCommand(match.Groups["gcode"].Value, init: false);
            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("Sends the provided gcode directly to UGS");
            if(helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine("Examples:");
                Console.WriteLine(" >F300 -- sends 'F300' to UGS");
            }
        }
    }
}
