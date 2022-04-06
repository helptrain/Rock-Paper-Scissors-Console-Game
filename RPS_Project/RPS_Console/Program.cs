
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
        private static int player1Points = 0;
        private static int player2Points = 0;
        private static IGame game = null;
        private string player = "player1";
        private string player2 = "player2";
        private static Program obj = new Program();
        private static HandSignalType[] choices = new HandSignalType[0];
        private static Dictionary<string, HandSignalType> plays = new Dictionary<string, HandSignalType>();
        private static EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);
        public static int counter = 0;
        public static bool retry = false;

        static void Main(string[] args)
        {
            if (obj.connectToGame())
            {
                Console.WriteLine("Welcome to Rock-Paper-Scissors game");
                Console.WriteLine("===================================\n");

                obj.PlayTheGame();
            }
            Console.ReadLine();
        }

        public void PlayTheGame() 
        {

            bool input = false;

            //game.SetPlayerHands(HandSignalType.None, HandSignalType.None);
            game.SetPlayerOneHand(HandSignalType.None);
            game.SetPlayerTwoHand(HandSignalType.None);

            choices = null;
            game.ResetChoices();
            while (!input)
            {
                //waitHandle.WaitOne();
                //waitHandle.WaitOne(); //?
                Console.WriteLine($"Welcome Player: {counter}");
                Console.WriteLine("Please choose:");
                Console.WriteLine("1. Rock\n2. Paper\n3. Scissors");

                do
                {
                    Console.Write("Please Choose an Option: ");
                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":
                            if (counter == 1)
                            {
                                game.PostChoice(player, HandSignalType.Rock);
                                retry = true;
                            }
                            else if (counter == 2)
                            {
                                game.PostChoice(player2, HandSignalType.Rock);
                                retry = true;
                            }
                            break;
                        case "2":
                            if (counter == 1)
                            {
                                game.PostChoice(player, HandSignalType.Paper);
                                retry = true;
                            }
                            else if (counter == 2)
                            {
                                game.PostChoice(player2, HandSignalType.Paper);
                                retry = true;
                            }
                            break;
                        case "3":
                            if (counter == 1)
                            {
                                game.PostChoice(player, HandSignalType.Scissors);
                                retry = true;
                            }
                            else if (counter == 2)
                            {
                                game.PostChoice(player2, HandSignalType.Scissors);
                                retry = true;
                            }
                            break;
                        default:
                            retry = false;
                            break;
                    }
                } while (!retry);
                waitHandle.WaitOne();

                plays = game.GetAllChoices();

                // try block for player 2 to connect in time
                try
                {
                    game.SetPlayerOneHand(plays["player1"]);
                    game.SetPlayerTwoHand(plays["player2"]);
                } catch (Exception e) {}
                
                string winner = game.Playing();
                Console.WriteLine($"The Winner of the round was {winner}");

                if (winner == "Player 1")
                {
                    player1Points++;
                }
                else if (winner == "Player 2")
                {
                    player2Points++;
                }
                

                Console.WriteLine($"Player 1 has accumulated {player1Points} points. Player 2 has accumulated {player2Points} points\n");

                plays.Clear();

                input = true;

            }
            Console.WriteLine("Do you want to play one more time(The player one must go first)?(y/n): ");
            string choiceToPlay = Console.ReadLine();
            switch (choiceToPlay)
            {
                case "y":                   
                    waitHandle.Reset();
                    obj.PlayTheGame();              
                    break;
                case "n":
                    if (counter == 1) game.Leave(player);
                    else if (counter == 2) game.Leave(player2);
                    Environment.Exit(0);
                    break;
            }         
        }

        // TODO: complete this 
        public void SendChoice(Dictionary<string, HandSignalType> dic)
        {
            try
            {
                foreach(var m in dic)
                {
                    //Console.WriteLine(m);
                }

                if (dic.Count > 1)
                {
                    waitHandle.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error in sendChoice: " + e);
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
                    //Console.WriteLine("get all msgs: " + game.GetAllChoices());
                    return true;
                }
                else if (game.Join(player2))
                {
                    counter = 2;
                    //Console.WriteLine("get all msgs: " + game.GetAllChoices());
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
