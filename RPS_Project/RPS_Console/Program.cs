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
        private static int id;
        private static ConcreteCallback cc = new ConcreteCallback();
        private static EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        private static List<Player> players = new List<Player>();
        private static Player player;


            static void Main(string[] args)
            {
                if(connectToGame())
                {
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("ERROR: Too Many Clients Connected");
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
            /*if (game != null)
            {
                // TODO: not sure what is supposed to be passed in 
                game.Leave(player);
                //game.Leave(player2);
                Console.WriteLine("exited");
            }      */       
        }
        // Sets up a connection to the WCF service and subscribes to the callback messages
        private static bool connectToGame()
        {
            try
            {
                // Configure the ABCs of using the MessageBoard service
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(cc, "GameService");

                game = channel.CreateChannel();

                id = game.Join();

                player = new Player($"Player {id}", id);
                players.Add(player);

                if(id == 404)
                {
                    return false;
                }

                Console.WriteLine("Welcome To the Game! ~\n");
                Console.WriteLine($"Welcome player {player.PlayerName}");

                return true;


            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
                return false;
            }
        }
    }
}
