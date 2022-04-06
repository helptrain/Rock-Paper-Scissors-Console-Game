using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS_Library
{
    public class Player
    {
        public string PlayerName;
        public int PlayerId;
        public HandSignalType HandSignal;
        public int score = 0;

        public Player(string name, int playerId)
        {
            PlayerName = name;
            PlayerId = playerId;
        }

        public void PlaySignal(HandSignalType playedtype)
        {
            HandSignal = playedtype;
        }
    }
}
