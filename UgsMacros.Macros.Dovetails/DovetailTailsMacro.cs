using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Macros.Cutting;

namespace UgsMacros.Macros.Dovetails
{
  [Macro("dovetail-tails")]
  public class DovetailTailsMacro : IHelpfulMacro
  {
    public string MatchString => @"^dovetail-tails\s+(?<args>.*)$";

    public bool Execute(ICommandSender commandSender, Match match)
    {
      var args = match.Groups["args"].Value.ParseAsCommandLine<DovetailTailsOptions>();

      var bitDiameter = args.BitDiameter.ToMillimeters().Value;
      var width = args.Width.ToMillimeters().Value;
      var cutDepth = args.CutDepth.ToMillimeters().Value;

      var patterns = args.Pattern
        .Split(',')
        .Select(e => e.Trim())
        .Select(e => new KeyValuePair<string, decimal>(
          e[0].ToString().ToLower(),
          e.Substring(1).Trim().ToMillimeters().Value))
        .ToArray();

      var offset = Math.Round(bitDiameter * 0.75m, 3);

      var commands = new List<Tuple<string, string>>();
      commands.Add(new Tuple<string, string>("Offset", $"Y-{offset}"));
      commands.Add(new Tuple<string, string>("Pluge", $"Z-{cutDepth}"));

      foreach (var pattern in patterns)
      {
        switch (pattern.Key)
        {
          case "s":
            Skip(commands, pattern.Value);
            break;

          case "c":
            Cut(commands, bitDiameter, offset, pattern.Value, width);
            break;

          default:
            throw new MacroFailedException($"Unrecognized pattern element: {pattern.Key}{pattern.Value}");
        }
      }

      commandSender.Init();
      foreach (var command in commands)
      {
        commandSender.SendLabeledCommand(command.Item1, command.Item2, init: false);
      }

      return true;
    }

    public void Help(HelpSummaryType helpSummaryType)
    {
      Console.WriteLine("Uses a dovetail bit to create dovetail tails");
      if(helpSummaryType == HelpSummaryType.Detailed)
      {
        Console.WriteLine();
        Console.WriteLine("Summary:");
        Console.WriteLine(" Stock must be mounted vertically.");
        Console.WriteLine(" It's recommended to run the same pattern at several cut-depths with a straight bit to clear");
        Console.WriteLine(" material prior to the final cut with the dovetail bit.");
        Console.WriteLine();
        Console.WriteLine(" --bit-diameter: Defines the widest part of the dovetail bit");
        Console.WriteLine(" --width: Defines the width of the wood");
        Console.WriteLine(" --cut-depth: Defines how deep to cut into the wood.");
        Console.WriteLine(" --pattern: The cutter pattern consisting of skip lengths (s) and cut lengths (c)");
        Console.WriteLine();
        Console.WriteLine("Example:");
        Console.WriteLine(" dovetail-tails --bit-diameter 0.5 --width 0.5 --cut-depth 0.25 --pattern s0.5, c1.25, s0.5, c1.25");
      }
    }

    private void Skip(List<Tuple<string, string>> commands, decimal amount)
    {
      commands.Add(new Tuple<string, string>("Skip", $"X{amount}"));
    }

    private void Cut(List<Tuple<string, string>> commands, decimal bitDiameter, decimal offset, decimal amount, decimal width)
    {
      var halfBitDiameter = Math.Round(bitDiameter * 0.5m, 3);
      var travel = amount - bitDiameter;
      var offsetWidth = offset + width;

      commands.Add(new Tuple<string, string>("Enter", $"X{halfBitDiameter}"));
      commands.Add(new Tuple<string, string>("Near Edge", $"Y{offsetWidth}"));
      commands.Add(new Tuple<string, string>("Near Edge", $"Y-{width}"));
      commands.Add(new Tuple<string, string>("Clear Near Side", $"X{travel}"));
      commands.Add(new Tuple<string, string>("Far Edge", $"Y{width}"));
      commands.Add(new Tuple<string, string>("Clear Far Side", $"X-{travel}"));
      commands.Add(new Tuple<string, string>("Clear Far Side", $"X{travel}"));
      commands.Add(new Tuple<string, string>("Far Edge", $"Y-{offsetWidth}"));
      commands.Add(new Tuple<string, string>("Exit", $"X{halfBitDiameter}"));

    }

  }
}
