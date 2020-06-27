using System;
using System.Collections.Generic;
using System.Text;
using UgsMacros.Framework;

namespace UgsMacros
{
    public class MacroVariableSet : Dictionary<string, object>, IMacroVariableSet
    {
        public MacroVariableSet()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }
    }
}
