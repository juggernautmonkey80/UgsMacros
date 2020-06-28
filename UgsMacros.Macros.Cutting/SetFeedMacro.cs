using System;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Framework.Regex;

namespace UgsMacros.Macros.Cutting
{
    [Macro("set-feed")]
    public class SetFeedMacro : IHelpfulMacro
    {
        private RegexDecimal _feed;
        private readonly IMacroVariableSet _variables;

        public SetFeedMacro(
            IMacroVariableSet variables)
        {
            _variables = variables;
            _feed = new RegexDecimal("feed");
        }

        public string MatchString => $@"^set-feed\s+{_feed.Expression}$";

        public bool Execute(ICommandSender commandSender, Match match, Func<string, bool?> translator)
        {
            var feed = Convert.ToInt32(_feed.GetValue(match));
            _variables.FeedRate(feed);
            commandSender.SendLabeledCommand("Feed", $"F{feed}", init: false);
            return true;
        }

        public void Help(HelpSummaryType helpSummaryType)
        {
            Console.WriteLine("Sets the standard feed rate");
            if (helpSummaryType == HelpSummaryType.Detailed)
            {
                Console.WriteLine("Examples:");
                Console.WriteLine(" set-feed 300 - Sets the feed to 300mpm");
            }
        }
    }
}
