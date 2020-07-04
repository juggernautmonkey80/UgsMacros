using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UgsMacros.Framework;
using UgsMacros.Macros.Cutting;

namespace UgsMacros.Macros.Dovetails
{
  [Macro("dovetail-pins")]
  public class DovetailPinsMacro : IHelpfulMacro
  {
    public string MatchString => @"^dovetail-pins\s+(?<args>.*)$";

    public bool Execute(ICommandSender commandSender, Match match)
    {
      var args = match.Groups["args"].Value.ParseAsCommandLine<DovetailPinsOptions>();

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

      var halfBitDiameter = Math.Round(bitDiameter * 0.5m, 3);

      var commands = new List<Tuple<string, string>>();
      commands.Add(new Tuple<string, string>("Offset", $"Y-{halfBitDiameter}"));
      commands.Add(new Tuple<string, string>("Pluge", $"Z-{cutDepth}"));

      foreach (var pattern in patterns)
      {
        switch (pattern.Key)
        {
          case "s":
            Clear(commands, bitDiameter, args.Angle, pattern.Value, width);
            break;

          case "c":
            Skip(commands, pattern.Value);
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
      Console.WriteLine("Uses a straight bit to make the dovetail pins.");
      if (helpSummaryType == HelpSummaryType.Detailed)
      {
        Console.WriteLine();
        Console.WriteLine("Summary:");
        Console.WriteLine(" Stock must be mounted vertically.");
        Console.WriteLine();
        Console.WriteLine(" --bit-diameter: Defines the width of the straight bit");
        Console.WriteLine(" --angle: Defines the angle of the bit used to cut the tails (not the current bit)");
        Console.WriteLine(" --width: Defines the width of the wood");
        Console.WriteLine(" --cut-depth: Defines how deep to cut into the wood.");
        Console.WriteLine(" --pattern: The cutter pattern consisting of clear lengths (s) and lengths to preserve (c)");
        Console.WriteLine("    Note that the s & c is the opposite from the tail macro.  This allows the same pattern to");
        Console.WriteLine("    be used for cutting the tails and pins.");
        Console.WriteLine();
        Console.WriteLine("Example:");
        Console.WriteLine(" dovetail-pins --bit-diameter 0.25 --angle 12 --width 0.5 --cut-depth 0.2 --pattern s0.5, c1.25, s0.5, c1.25");
      }
    }

    private void Skip(List<Tuple<string, string>> commands, decimal amount)
    {
      commands.Add(new Tuple<string, string>("Skip", $"X{amount}"));
    }

    private void Clear(List<Tuple<string, string>> commands, decimal bitDiameter, int angle, decimal amount, decimal width)
    {
      var halfBitDiameter = Math.Round(bitDiameter * 0.5m, 3);
      var travel = amount - bitDiameter;

      commands.Add(new Tuple<string, string>("Preposition", $"X{halfBitDiameter}"));
      commands.Add(new Tuple<string, string>("Preposition", $"Y{halfBitDiameter}"));

      var opp = Math.Round(width * Convert.ToDecimal(Math.Tan(angle * Math.PI / 180.0)), 3);

      commands.Add(new Tuple<string, string>("Near Edge", $"X-{opp} Y{width}"));
      commands.Add(new Tuple<string, string>("Near Edge", $"X{opp} Y-{width}"));
      commands.Add(new Tuple<string, string>("Move Far", $"X{travel}"));
      commands.Add(new Tuple<string, string>("Far Edge", $"X{opp} Y{width}"));
      commands.Add(new Tuple<string, string>("Far Edge", $"X-{opp} Y-{width}"));

      commands.Add(new Tuple<string, string>("Exit", $"Y-{halfBitDiameter}"));
      commands.Add(new Tuple<string, string>("Exit", $"X{halfBitDiameter}"));
    }
  }
}
