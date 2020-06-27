using System;
using System.Collections.Generic;
using System.Text;

namespace UgsMacros.Framework
{
    public static class CommandSenderExtensions
    {
        public static void SendLabeledCommand(this ICommandSender sender, string label, string command)
        {
            Console.Write($"{label} ");
            sender.SendCommand(command);
        }
    }
}
