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

        public void SetPlayerHands(HandSignalType p1, HandSignalType p2)
        {
            playerOne.PlaySignal(p1);
            playerTwo.PlaySignal(p2);
        }

        public HandSignalType[] GetAllChoices()
        {
            return choices.ToArray<HandSignalType>();
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
            return true;
        }
        public void Leave(string clientName)
        {
            if (callbacks.ContainsKey(clientName.ToUpper()))
            {
                callbacks.Remove(clientName.ToUpper());
            }
        }

        public void PostChoice(HandSignalType choice)
        {
            choices.Insert(0, choice);
            UpdateAllPlayers();
        }

        //Triggers callback
        private void UpdateAllPlayers()
        {
            HandSignalType[] choiceArr = choices.ToArray<HandSignalType>();
            foreach (ICallback c in callbacks.Values)
            {
                c.SendChoice(choiceArr);
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

            return $"The winner is: {winner.PlayerName}";

        }
    }
}
