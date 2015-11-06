using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBackuperService
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                BackuperService service1 = new BackuperService();
                service1.TestStartupAndStop(args);
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new BackuperService()
                };
                ServiceBase.Run(ServicesToRun);
            }
           
        }
    }
}
