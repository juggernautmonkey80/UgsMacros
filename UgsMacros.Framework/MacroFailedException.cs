using System;

namespace UgsMacros.Framework
{
    public class MacroFailedException : Exception
    {
        public MacroFailedException(string message)
            : base(message)
        {
        }
    }
}
