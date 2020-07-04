﻿using CommandLine;

namespace UgsMacros.Macros.Dovetails
{
  public class DovetailPinsOptions
  {
    [Option("bit-diameter", Required = true, HelpText = "Defines the bit diameter.")]
    public string BitDiameter { get; set; }

    [Option("angle", Required = true, HelpText = "the angle of the dovetail bit")]
    public int Angle { get; set; }

    [Option("width", Required = true, HelpText = "The width of the wood")]
    public string Width { get; set; }

    [Option("cut-depth", Required = true, HelpText = "how deep to cut the wood")]
    public string CutDepth { get; set; }

    [Option("pattern", Required = true, HelpText = "The cutting pattern")]
    public string Pattern { get; set; }
  }
}
