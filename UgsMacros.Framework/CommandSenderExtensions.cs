using System;

namespace UgsMacros.Framework
{
    public static class CommandSenderExtensions
    {
        public static void Init(this ICommandSender sender)
        {
            sender.SendCommand(string.Empty);
        }

        public static void SendLabeledCommand(this ICommandSender sender, string label, string command, bool init = true)
        {
            if (init)
            {
                sender.Init();
            }

            Console.Write($" {label} ");
            sender.SendCommand(command, init: false);
        }
    }
}
