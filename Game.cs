using System;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class Game
    {
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }

        public Game(Team homeTeam, Team awayTeam)
        {
            HomeTeam = homeTeam;
            AwayTeam = awayTeam;
        }

        public string Comparer
        {
            get
            {
                if (string.Compare(HomeTeam.Name, AwayTeam.Name) > 0)
                {
                    return $"{HomeTeam.Name}-{AwayTeam.Name}";
                }
                else
                {
                    return $"{AwayTeam.Name}-{HomeTeam.Name}";
                }
            }
        }
    }
}
