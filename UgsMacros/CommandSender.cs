using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using UgsMacros.Framework;

namespace UgsMacros
{
    public class CommandSender : ICommandSender
    {
        private readonly RestClient _restClient;

        public CommandSender()
        {
            var pendantUrl = ConfigurationManager.AppSettings["PendantUrl"];
            _restClient = new RestClient(pendantUrl);
        }

        public void SendCommand(string command)
        {
            Console.Write($":: {command} -- ");
            var request = new RestRequest("api/v1/machine/sendGcode", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { commands = command });
            var response = _restClient.Execute(request);
            Console.WriteLine($"{response.StatusCode}");
        }
    }
}
