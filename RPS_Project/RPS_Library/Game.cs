using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RPS_Library
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Game : IGame
    {
        private const int MAX_CLIENTS = 2; //TODO: Change this if needed

        private Dictionary<string, ICallback> callbacks = new Dictionary<string, ICallback>();
        private Dictionary<string, HandSignalType> playerChoices = new Dictionary<string, HandSignalType>();
        private List<HandSignalType> choices = new List<HandSignalType>();

        public Player playerOne = new Player("Player 1", 1);
        public Player playerTwo = new Player("Player 2" , 2);

        public Player winner;

        // needed for loading as a service
        public Game() { }

        public Game(Player p1, Player p2)
        {
            playerOne = p1;
            playerTwo = p2;
        }

        public void SetPlayerOneHand(HandSignalType p)
        {
            playerOne.PlaySignal(p);
        }

        public void SetPlayerTwoHand(HandSignalType p)
        {
            playerTwo.PlaySignal(p);
        }

        public Dictionary<string, HandSignalType> GetAllChoices()
        {
            return playerChoices;
        }

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

        public void Leave(string clientName)
        {
            if(callbacks.ContainsKey(clientName.ToUpper()))
            {
                callbacks.Remove(clientName.ToUpper());
                Console.WriteLine(clientName + " left");
            }
            /*
            ICallback cb = OperationContext.Current.GetCallbackChannel<ICallback>();

            if(callbacks.ContainsValue(cb))
            {
                int i = callbacks.Values.ToList().IndexOf(cb);
                string id = callbacks.ElementAt(i).Key;
                callbacks.Remove(id);
                Console.WriteLine("Service disconnected - unregistered from callbacks");
            }
            */
        }

        public void PostChoice(string playerName, HandSignalType play)
        {
            playerChoices.Add(playerName, play);
            UpdateAllPlayers();
        }

        public void ResetChoices()
        {
            choices.Clear();
        }

        //Triggers callback
        private void UpdateAllPlayers()
        {
            foreach (ICallback c in callbacks.Values)
            {
                c.SendChoice(playerChoices);
            }
        }

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
