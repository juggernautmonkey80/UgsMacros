using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using UgsMacros.Framework;

namespace UgsMacros.Macros.Jogging
{
    public static class RestClientExtensions
    {
        public static void SendJogCommand(this ICommandSender restClient, string command)
        {
            restClient.SendCommand("G00");
            restClient.SendCommand(command);
            restClient.SendCommand("G01");
        }
    }
}
