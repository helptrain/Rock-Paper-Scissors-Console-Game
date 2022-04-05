
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
        private string player2 = "Adam";
        private static Program obj = new Program();
        private static HandSignalType[] choices = new HandSignalType[0];
        private static EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        public static int counter = 0;

        static void Main(string[] args)
        {
            bool input = false;
            if (obj.connectToGame())
            {

                Console.WriteLine("Welcome to Rock-Paper-Scissors game");
                Console.WriteLine("===================================");

                while (!input)
                {

                    //waitHandle.WaitOne(); //?
                    Console.WriteLine($"Welcome Player: {counter}");
                    Console.WriteLine("Please choose:");
                    Console.WriteLine("1. Rock\n2. Paper\n3. Scissors");

                    Console.Write("Please Choose an Option: ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            game.PostChoice(HandSignalType.Rock);
                            break;
                        case "2":
                            game.PostChoice(HandSignalType.Paper);
                            break;
                        case "3":
                            game.PostChoice(HandSignalType.Scissors);
                            break;
                    }
                    waitHandle.WaitOne();

                    choices = game.GetAllChoices();

                    game.SetPlayerHands(choices[1], choices[0]);

                    Console.WriteLine(game.Playing());

                    input = true;

                }
            }
            Console.ReadLine();
        }


        // TODO: complete this 
        public void SendChoice(HandSignalType[] messages)
        {
            try
            {
                foreach(var m in messages)
                {
                    Console.WriteLine(m);
                }

                if (messages.Length > 1)
                {
                    waitHandle.Set();
                }
            }
            catch (Exception e)
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
        private bool connectToGame()
        {
            try
            {
                // Configure the ABCs of using the MessageBoard service
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "GameService");

                game = channel.CreateChannel();

                if (game.Join(player))
                {
                    counter = 1;
                    Console.WriteLine("get all msgs: " + game.GetAllChoices());
                    return true;
                }
                else if (game.Join(player2))
                {
                    counter = 2;
                    Console.WriteLine("get all msgs: " + game.GetAllChoices());
                    return true;
                }
                else
                {
                    // Alias rejected by the service so nullify service proxies
                    game = null;
                    Console.WriteLine("ERROR: Alias in use. Please try again.");
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error: " + e.Message);
                return false;
            }
        }
    }
}
