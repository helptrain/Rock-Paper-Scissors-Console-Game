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
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }

        }
    }
}
