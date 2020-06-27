using System;
using System.Collections.Generic;
using System.Text;

namespace UgsMacros.Framework
{
    public interface IHelpfulMacro : IMacro
    {
        void Help(HelpSummaryType helpSummaryType);
    }
}
