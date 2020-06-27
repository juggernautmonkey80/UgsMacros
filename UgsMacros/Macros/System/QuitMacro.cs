using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.System
{
    public class QuitMacro : IMacro
    {
        public string MatchString => "quit";

        public bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator)
        {
            return false;
        }
    }
}
