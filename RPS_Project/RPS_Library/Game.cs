using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS_Library
{
    public class Game
    {
        public Player playerOne;
        public Player playerTwo;

        public Player winner;

        public Game(Player p1, Player p2)
        {
            playerOne = p1;
            playerTwo = p2;
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
