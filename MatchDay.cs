using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class MatchDay
    {
        public DateTime Date { get; set; }
        public Game[] Games { get; set; }

        public static Random Randomizer = new Random();

        public MatchDay(DateTime date)
        {
            Date = date;
        }

        public void GenerateGames(Team[] teamsPlayingThisMatchDay)
        {
            // Add games until the number of teams yet to schedule for this MatchDay is either 0 or 1 (if odd number of non-bye teams)
            List<Game> games = new List<Game>();
            while (teamsPlayingThisMatchDay.Length >= 2)
            {
                int homeTeam = Randomizer.Next(0, teamsPlayingThisMatchDay.Length);
                int awayTeam = Randomizer.Next(0, teamsPlayingThisMatchDay.Length);
                if (homeTeam != awayTeam)
                {
                    // We found 2 different teams, have them play each other
                    games.Add(new Game(teamsPlayingThisMatchDay[homeTeam], teamsPlayingThisMatchDay[awayTeam]));

                    // Remove those teams from the teams for subsequent matches on this match day
                    teamsPlayingThisMatchDay = teamsPlayingThisMatchDay.Where(t => t.Name != teamsPlayingThisMatchDay[homeTeam].Name && t.Name != teamsPlayingThisMatchDay[awayTeam].Name).ToArray();
                }
            }
            Games = games.ToArray();
        }

        public override string ToString()
        {
            StringBuilder text = new StringBuilder();
            if (Games != null)
            {
                for (int i = 0; i < Games.Length; i++)
                {
                    Game game = Games[i];
                    text.AppendLine($"{Date:yyyy-MM-dd} @{Season.MatchTimeSlots[i]}: {game.HomeTeam.Name} v {game.AwayTeam.Name}");
                }
            }
            else
            {
                text.AppendLine($"{Date:yyyy-MM-dd}: UNSCHEDULED");
            }
            return text.ToString();
        }
    }
}
