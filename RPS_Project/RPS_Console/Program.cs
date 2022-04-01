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
    public partial class Program : ICallback
    {
        // Member variables
        private static IGame game = null;
        private string player = "John";
        //private string player2 = "Adam";

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

        // TODO: complete this 
        public void SendChoice(HandSignalType[] messages)
        {
            try
            {
                Console.WriteLine("sendChoice: " + messages);
            } catch(Exception e)
            {
                Console.WriteLine("error in sendChoice: " + e);
            }
        }
        

        private void UserExited()
        {
            if (game != null)
            {
                // TODO: not sure what is supposed to be passed in 
                game.Leave(player);
                //game.Leave(player2);
                Console.WriteLine("exited");
            }             
        }

        // Sets up a connection to the WCF service and subscribes to the callback messages
        private void connectToGame()
        {
            try
            {
                // Configure the ABCs of using the MessageBoard service
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "GameService");

                game = channel.CreateChannel();

                if (game.Join(player))
                {
                    Console.WriteLine("get all msgs: " + game.GetAllChoices());
                }
                else
                {
                    // Alias rejected by the service so nullify service proxies
                    game = null;
                    Console.WriteLine("ERROR: Alias in use. Please try again.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
            }
        }
    }
}
