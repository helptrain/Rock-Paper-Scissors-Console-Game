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

        private Dictionary<int, ICallback> callbacks = new Dictionary<int, ICallback>();
        private List<HandSignalType> choices = new List<HandSignalType>();

        public Player playerOne;
        public Player playerTwo;

        private int nextPlayer;

        public Player winner;

        // needed for loading as a service
        public Game() { }

        public Game(Player p1, Player p2)
        {
            playerOne = p1;
            playerTwo = p2;
        }
        public HandSignalType[] GetAllChoices()
        {
            return choices.ToArray<HandSignalType>();
        }
        public int Join()
        {
            ICallback callBack = OperationContext.Current.GetCallbackChannel<ICallback>();
            if(callbacks.ContainsValue(callBack))
            {
                int i = callbacks.Values.ToList().IndexOf(callBack);
                return callbacks.Keys.ElementAt(i);
            }

            if(callbacks.Count == 2)
            {
                return 404;
            }

            callbacks.Add(nextPlayer, callBack);
            return nextPlayer++;
        }
        public void Leave(string clientName)
        {
            /*if (callbacks.ContainsKey(clientName.ToUpper()))
            {
                callbacks.Remove(clientName.ToUpper());
            }*/
        }

        public void PostChoice(HandSignalType choice)
        {
            choices.Insert(0, choice);
            UpdateAllPlayers();
        }

        //Triggers callback
        private void UpdateAllPlayers()
        {
            /*HandSignalType[] choiceArr = choices.ToArray<HandSignalType>();
            callbacks.Values.ToList<IGame>().ForEach(cb => cb.SendChoice(choiceArr));
            foreach(ICallback c in callbacks.Values)
            {
                c.SendChoice(choiceArr);
            }*/
        }

        public string Playing()
        {
            if(playerOne.HandSignal == HandSignalType.Rock && playerTwo.HandSignal == HandSignalType.Scissors)
            {
                this.winner = playerOne;
            }
            else if(playerOne.HandSignal == HandSignalType.Paper && playerTwo.HandSignal == HandSignalType.Rock)
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
