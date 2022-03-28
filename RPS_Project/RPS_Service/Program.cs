using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using RPS_Library;

namespace RPS_Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost servHost = null;

            try
            {
                // Register the service address
                servHost = new ServiceHost(typeof(Game), new Uri("net.tcp://localhost:40000/RPS_Library/"));

                // Register the service contract and binding
                servHost.AddServiceEndpoint(typeof(IGame), new NetTcpBinding(), "GameService");

                // Run the service
                servHost.Open();

                Console.WriteLine("Service started. Please any key to quit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Wait for a keystroke
                Console.ReadKey();
                if (servHost != null)
                    servHost.Close();
            }

        }
    }
}
