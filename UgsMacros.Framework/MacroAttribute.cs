using System;

namespace UgsMacros.Framework
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MacroAttribute : Attribute
    {
        public MacroAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
