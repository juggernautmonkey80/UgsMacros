using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.System
{
    public class ExitMacro : IMacro
    {
        public string MatchString => "exit";

        public bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator)
        {
            return false;
        }
    }
}
