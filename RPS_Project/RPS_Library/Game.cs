/**
 * Project Name: RPS_Library
 * File Name: Game.cs
 * Author(s): L. Bas, S. Podkorytov, M. Ivanov, T. Pollard
 * Date: 2022-04-06
 * Context: Actual gameplay api that the client will connect to and use
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace RPS_Library
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        private const int MAX_CLIENTS = 2;

        private Dictionary<string, ICallback> callbacks = new Dictionary<string, ICallback>();
        private Dictionary<string, HandSignalType> playerChoices = new Dictionary<string, HandSignalType>();

        public Player playerOne = new Player("Player 1", 1);
        public Player playerTwo = new Player("Player 2" , 2);
        public Player winner;
        
        public Game() { }

        public Game(Player p1, Player p2)
        {
            playerOne = p1;
            playerTwo = p2;
        }

        /**
         * Method: SetPlayerOneHand
         * Accepts: HandSignalType
         * Returns: void
         */
        public void SetPlayerOneHand(HandSignalType p)
        {
            playerOne.PlaySignal(p);
        }

        /**
         * Method: SetPlayerTwoHand
         * Accepts: HandSignalType
         * Returns: void
         */
        public void SetPlayerTwoHand(HandSignalType p)
        {
            playerTwo.PlaySignal(p);
        }

        /**
         * Method: GetAllChoices
         * Accepts: void
         * Returns: Dictionary of string & HandSignalType
         */
        public Dictionary<string, HandSignalType> GetAllChoices()
        {
            return playerChoices;
        }

        /**
         * Method: Join
         * Accepts: string
         * Returns: boolean
         */
        public bool Join(string clientName)
        {
            if (callbacks.ContainsKey(clientName.ToUpper()))
            {
                return false;
            }
            if (callbacks.Count >= MAX_CLIENTS)
            {
                return false;
            }

            ICallback callBack = OperationContext.Current.GetCallbackChannel<ICallback>();
            callbacks.Add(clientName.ToUpper(), callBack);
            Console.WriteLine(clientName + " joined");
            return true;
        }

        /**
         * Method: Leave
         * Accepts: string
         * Returns: void
         */
        public void Leave(string clientName)
        {
            if(callbacks.ContainsKey(clientName.ToUpper()))
            {
                callbacks.Remove(clientName.ToUpper());
                if (playerChoices.ContainsKey(clientName))
                {
                    playerChoices.Remove(clientName);
                }
                Console.WriteLine(clientName + " left");
            }
        }

        /**
         * Method: PostChoice
         * Accepts: string, HandSignalType
         * Returns: void
         */
        public void PostChoice(string playerName, HandSignalType play)
        {
            try
            {
                playerChoices.Add(playerName, play);
                UpdateAllPlayers();
            } 
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }         
        }

        /**
         * Method: ResetChoices
         * Accepts: void
         * Returns: void
         */
        public void ResetChoices()
        {
            playerChoices.Clear();
        }

        /**
         * Method: UpdateAllPlayers
         * Accepts: void
         * Returns: void
         */
        //Triggers callback
        private void UpdateAllPlayers()
        {
            foreach (ICallback c in callbacks.Values)
            {
                c.SendChoice(playerChoices);
            }
        }

        /**
         * Method: Playing
         * Accepts: void
         * Returns: string
         */
        public string Playing()
        {
            if (playerOne.HandSignal == HandSignalType.Rock && playerTwo.HandSignal == HandSignalType.Scissors)
            {
                this.winner = playerOne;
            }
            else if (playerOne.HandSignal == HandSignalType.Paper && playerTwo.HandSignal == HandSignalType.Rock)
            {
                this.winner = playerOne;
            }
            else if (playerOne.HandSignal == HandSignalType.Scissors && playerTwo.HandSignal == HandSignalType.Paper)
            {
                this.winner = playerOne;
            }
            else if (playerOne.HandSignal == HandSignalType.Rock && playerTwo.HandSignal == HandSignalType.Paper)
            {
                this.winner = playerTwo;
            }
            else if (playerOne.HandSignal == HandSignalType.Paper && playerTwo.HandSignal == HandSignalType.Scissors)
            {
                this.winner = playerTwo;
            }
            else if (playerOne.HandSignal == HandSignalType.Scissors && playerTwo.HandSignal == HandSignalType.Rock)
            {
                this.winner = playerTwo;
            }
            else if(playerOne.HandSignal == playerTwo.HandSignal)
            {
                return "Draw!";
            }

            playerChoices.Clear();

            return winner.PlayerName;
        }
    }
}
