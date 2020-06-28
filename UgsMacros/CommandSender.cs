using RestSharp;
using System;
using System.Configuration;
using UgsMacros.Framework;

namespace UgsMacros
{
    public class CommandSender : ICommandSender
    {
        private readonly IInitCommand[] _initCommands;
        private readonly RestClient _restClient;

        public CommandSender(
            IInitCommand[] initCommands)
        {
            _initCommands = initCommands;

            var pendantUrl = ConfigurationManager.AppSettings["PendantUrl"];
            _restClient = new RestClient(pendantUrl);
        }

        public void SendCommand(string command, bool init = true)
        {
            if (init)
            {
                foreach (var initCommand in _initCommands)
                {
                    Console.Write(" (init) ");
                    SendCommand(initCommand.GCode, init: false);
                }
            }

            if (!string.IsNullOrWhiteSpace(command))
            {
                Console.Write($":: {command} -- ");
                var request = new RestRequest("api/v1/machine/sendGcode", Method.POST, DataFormat.Json);
                request.AddJsonBody(new { commands = command });
                var response = _restClient.Execute(request);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    Console.WriteLine($"OK");
                }
                else
                {
                    Console.WriteLine($"{response.StatusCode}");
                }
            }
        }
    }
}
