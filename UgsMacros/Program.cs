using Ninject;

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
