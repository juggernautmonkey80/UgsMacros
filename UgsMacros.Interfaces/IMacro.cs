using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UgsMacros.Interfaces
{
    public interface IMacro
    {
        string MatchString { get; }

        bool Execute(IRestClient restClient, Match match);
    }
}
