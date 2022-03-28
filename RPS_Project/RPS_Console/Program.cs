using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;  // WCF types
using System.Threading;
using System.Runtime.InteropServices;   // Need this for DllImport()
using RPS_Library;

namespace RPS_Console
{
    public class Program
    {
        // Member variables
        private static IGame game = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Test");

            try
            {
                ChannelFactory<IGame> channel = new ChannelFactory<IGame>(
                new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:40000/RPS_Library/GameService"));

                game = channel.CreateChannel();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

        }
    }
}
