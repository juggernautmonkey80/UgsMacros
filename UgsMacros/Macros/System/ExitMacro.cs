using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.System
{
    [Macro("exit|quit")]
    public class ExitMacro : IHelpfulMacro
    {
        public string MatchString => "^(exit|quit)$";

        public bool Execute(ICommandSender commandSender, Match match)
        {
            return false;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("exits the program");
        }
    }
}
