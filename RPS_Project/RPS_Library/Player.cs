/**
 * Project Name: RPS_Library
 * File Name: Player.cs
 * Author(s): L. Bas, S. Podkorytov, M. Ivanov, T. Pollard
 * Date: 2022-04-06
 * Context: Holds Player properties and methods
 */

using System;

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

        /**
         * Method: PlaySignal
         * Accepts: HandSignalType
         * Returns: void
         */
        public void PlaySignal(HandSignalType playedtype)
        {
            HandSignal = playedtype;
        }
    }
}
