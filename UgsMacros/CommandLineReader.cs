﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using UgsMacros.Framework;

namespace UgsMacros
{
    public class CommandLineReader : ICommandLineReader
    {
        private readonly ICommandSender _commandSender;
        private readonly IMacro[] _macros;

        public CommandLineReader(
            ICommandSender commandSender,
            IMacro[] macros)
        {
            _commandSender = commandSender;
            _macros = macros;
        }

        public void Run()
        {
            _commandSender.SendCommand("G21 G91 G01 F300");

            while (true)
            {
                Console.Write(@"UGS:\>");
                var cmd = Console.ReadLine();
                var shouldContine = ProcessCommand(cmd);

                if (!shouldContine.HasValue)
                {
                    Console.WriteLine("Unknown macro");
                }

                if (!shouldContine.GetValueOrDefault(true))
                {
                    return;
                }
            }
        }

        private bool? ProcessCommand(string cmd)
        {
            bool? shouldContine = null;
            foreach (var macro in _macros)
            {
                var match = Regex.Match(cmd, macro.MatchString, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    shouldContine = macro.Execute(_commandSender, match, ProcessCommand);
                    break;
                }
            }

            return shouldContine;
        }
    }
}
