/**
 * Project Name: RPS_Service
 * File Name: Program.cs
 * Author(s): L. Bas, S. Podkorytov, M. Ivanov, T. Pollard
 * Date: 2022-04-06
 * Context: Starts up the service provider
 */

using System;
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
