using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace UgsMacros
{
    public static class RestClientExtensions
    {
        public static void SendCommand(this IRestClient restClient, string command)
        {
            Console.Write($":: {command} -- ");
            var request = new RestRequest("api/v1/machine/sendGcode", Method.POST, DataFormat.Json);
            request.AddJsonBody(new { commands = command });
            var response = restClient.Execute(request);
            Console.WriteLine($"{response.StatusCode}");
        }
    }
}
