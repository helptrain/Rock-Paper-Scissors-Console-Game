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
        public HandSignalType HandSignal;

        public Player(string name)
        {
            PlayerName = name;
        }

        public void PlaySignal(HandSignalType playedtype)
        {
            HandSignal = playedtype;
        }
    }
}
