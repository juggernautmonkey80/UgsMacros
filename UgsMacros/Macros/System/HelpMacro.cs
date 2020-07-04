using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros.Macros.System
{
    [Macro("help[ MACRO_NAME]")]
    public class HelpMacro : IMacro
    {
        private IReadOnlyDictionary<string, IHelpfulMacro> _helpfulMacros;

        public HelpMacro(
            IHelpfulMacro[] helpfulMacros)
        {
            _helpfulMacros = helpfulMacros
                .Select(
                    m => new
                    {
                        Key = m.GetType().GetCustomAttributes<MacroAttribute>().FirstOrDefault()?.Name,
                        Macro = m
                    })
                .Where(a => !string.IsNullOrWhiteSpace(a.Key))
                .ToDictionary(
                    a => a.Key,
                    a => a.Macro,
                    StringComparer.InvariantCultureIgnoreCase);
        }

        public string MatchString => @"^(help|\?)(?<detail>\s+\S+)*";

        public bool Execute(ICommandSender commandSender, Match match)
        {
            var targetMacroName = Convert.ToString(match.Groups["detail"].Value).Trim();
            if (targetMacroName.Length > 0)
            {
                if (_helpfulMacros.TryGetValue(targetMacroName, out IHelpfulMacro targetMacro))
                {
                    Console.WriteLine($"--- {targetMacroName} ---");
                    targetMacro.Help(HelpSummaryType.Detailed);
                }
                else
                {
                    Console.WriteLine("** Unknown helpful macro");
                    ListBasicHelp();
                }
            }
            else
            {
                ListBasicHelp();
            }

            return true;
        }

        private void ListBasicHelp()
        {
            Console.WriteLine("help MACRO_NAME - Gets more details about the macro usage");
            foreach (var helpfulMacro in _helpfulMacros.OrderBy(t => t.Key))
            {
                Console.Write($"{helpfulMacro.Key} - ");
                helpfulMacro.Value.Help(HelpSummaryType.Basic);
            }
        }
    }
}
