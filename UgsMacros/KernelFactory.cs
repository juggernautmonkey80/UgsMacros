using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using UgsMacros.Framework;
using UgsMacros.Macros.System;

namespace UgsMacros
{
    public class KernelFactory
    {
        public IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<ICommandSender>().To<CommandSender>();
            kernel.Bind<ICommandLineReader>().To<CommandLineReader>();
            kernel.Bind<IMacro>().To<HelpMacro>();
            kernel.Bind<IMacro>().To<ExitMacro>();
            kernel.Bind<IHelpfulMacro>().To<ExitMacro>();
            kernel.Bind<IMacro>().To<SendMacro>();
            kernel.Bind<IHelpfulMacro>().To<SendMacro>();
            

            var pluginAssemblyNames = ConfigurationManager.AppSettings["Register"]
                .Split(',')
                .Where(t => !string.IsNullOrWhiteSpace(t))
                .Select(t => ConfigurationManager.AppSettings[$"Register.{t}"])
                .ToList();

            foreach (var pluginAssemblyName in pluginAssemblyNames)
            {
                Console.WriteLine($"* Registering Plugin {pluginAssemblyName}");
                var macroTypes = Assembly.Load(pluginAssemblyName)
                    .GetTypes()
                    .Where(t => typeof(IMacro).IsAssignableFrom(t))
                    .Where(t => t.GetCustomAttributes<MacroAttribute>().Any())
                    .ToList();

                foreach (var macroType in macroTypes)
                {
                    var macroAttribute = macroType.GetCustomAttributes<MacroAttribute>().First();
                    Console.WriteLine($"Registering Macro '{macroAttribute.Name}' ({macroType})");

                    kernel.Bind<IMacro>().To(macroType);

                    if (typeof(IHelpfulMacro).IsAssignableFrom(macroType))
                    {
                        kernel.Bind<IHelpfulMacro>().To(macroType);
                    }
                }
            }

            return kernel;
        }
    }
}
