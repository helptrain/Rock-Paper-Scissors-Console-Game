/**
 * Project Name: RPS_Console
 * File Name: Program.cs
 * Author(s): L. Bas, S. Podkorytov, M. Ivanov, T. Pollard
 * Date: 2022-04-06
 * Context: Actual gameplay logic that will connect to the game api 
 */

using System;
using System.Windows;
using System.Collections.Generic;
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
        private static Dictionary<string, HandSignalType> plays = new Dictionary<string, HandSignalType>();
        private static EventWaitHandle waitHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

        public static int counter = 0;
        public static bool retry = false;

        static void Main(string[] args)
        {
            if (obj.ConnectToGame())
            {
                Console.WriteLine("Welcome to Rock-Paper-Scissors!");
                Console.WriteLine("===================================\n");

                obj.PlayTheGame();
            }
            Console.ReadLine();
        }

<<<<<<< HEAD
        // test

=======
        /**
         * Method: PlayTheGame()
         * Accepts: void
         * Returns: void
         */
>>>>>>> d661771 (Added commenting and code cleanup)
        public void PlayTheGame() 
        {

            bool input = false;

            game.SetPlayerOneHand(HandSignalType.None);
            game.SetPlayerTwoHand(HandSignalType.None);

            game.ResetChoices();
            Console.WriteLine($"Welcome Player {counter}\n");
            while (!input)
            {
                Console.WriteLine("Please choose: (1,2,3)");
                Console.WriteLine("1. Rock\n2. Paper\n3. Scissors");

                do
                {
                    Console.Write("Please Choose an Option: ");
                    string choice = Console.ReadLine();
                    Console.WriteLine("You choose " + choice + "\n");
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

                try
                {
                    game.SetPlayerOneHand(plays["player1"]);
                    game.SetPlayerTwoHand(plays["player2"]);
                    game.ResetChoices();
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
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

                input = true;
            }


            Console.WriteLine("Do you want to play one more time? (y/n): ");
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

        /**
         * Method: SendChoice
         * Accepts: Dictionary of string & HandSignalType
         * Returns: void
         */
        public void SendChoice(Dictionary<string, HandSignalType> dic)
        {
            try
            {
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

        /**
         * Method: ConnectToGame()
         * Accepts: void
         * Returns: boolean
         */
        // Sets up a connection to the WCF service and subscribes to the callback messages
        private bool ConnectToGame()
        {
            try
            {
                DuplexChannelFactory<IGame> channel = new DuplexChannelFactory<IGame>(this, "GameService");

                game = channel.CreateChannel();

                if (game.Join(player))
                {
                    counter = 1;
                    return true;
                }
                else if (game.Join(player2))
                {
                    counter = 2;
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
