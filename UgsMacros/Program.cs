using System;
using System.Text.RegularExpressions;
using Ninject;
using RestSharp;
using UgsMacros.Macros;
using UgsMacros.Macros.Cutting;
using UgsMacros.Macros.Jogging;
using UgsMacros.Macros.System;

namespace UgsMacros
{
    class Program
    {
        static void Main(string[] args)
        {
            new KernelFactory()
                .CreateKernel()
                .Get<ICommandLineReader>()
                .Run();
        }
    }
}
