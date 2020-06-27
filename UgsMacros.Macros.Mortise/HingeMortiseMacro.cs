using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.Mortise
{
    // [Macro("Hinge-Mortise")]
    public class HingeMortiseMacro // : IMacro
    {
        public string MatchString => @"^hinge-mortise\s+(?<dir>[-]*)(?<axis>[xy]+)\s+(?<w>\d)x(?<h>\d)\s+(?<d>\d)";

        public bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator)
        {
            throw new NotImplementedException();
        }
    }
}
