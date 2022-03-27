using CMIUICXCore.Code.Starter;
using CMIUICXCore.Starter;
using System;

namespace CMIUICXCore
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var manager = new StarterManager(new WindowsStarter<Startup>(), new LinuxStarter<Startup>());
                manager.Start(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
