using System;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class Matchup
    {
        public string Comparer { get; set; }
        public List<Game> Games { get; set; }

        public Matchup(string comparer)
        {
            Comparer = comparer;
            Games = new List<Game>();
        }

        public void AddGame(Game game)
        {
            Games.Add(game);
        }

        public int MatchupCount
        {
            get
            {
                return Games.Count;
            }
        }
    }
}
