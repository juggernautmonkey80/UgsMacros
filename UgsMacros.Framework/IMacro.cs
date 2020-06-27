using System;
using System.Text.RegularExpressions;

namespace UgsMacros.Framework
{
    public interface IMacro
    {
        string MatchString { get; }

        bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator);
    }
}
