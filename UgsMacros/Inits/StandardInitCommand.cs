using UgsMacros.Framework;

namespace UgsMacros.Inits
{
    public class StandardInitCommand : IInitCommand
    {
        private readonly IMacroVariableSet _variables;

        public StandardInitCommand(IMacroVariableSet variables)
        {
            _variables = variables;
        }

        public string GCode => $"G21 G91 G01 F{_variables.FeedRate()}";
    }
}
