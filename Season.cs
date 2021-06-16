using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EbotsScheduler
{
    class Season
    {
        /// <summary>
        /// How many iterations to try before giving up (can run 100k in ~10 seconds)
        /// </summary>
        public const int MAX_TRIES = 100000;

        /// <summary>
        /// What is the highest percentage of playing in the same time slot that a team should have
        /// </summary>
        public const int MAX_TIMESLOT_PERCENT = 57;

        /// <summary>
        /// List of the teams in the league, and each team's bye week(s)
        /// </summary>
        public readonly static LeagueTeams LeagueTeams = new LeagueTeams(
            new Team("Blue", new DateTime(2021, 6, 19), new DateTime(2021, 7, 24), new DateTime(2021, 8, 28))
            , new Team("White", new DateTime(2021, 6, 26), new DateTime(2021, 7, 31))
            , new Team("Red", new DateTime(2021, 7, 3), new DateTime(2021, 8, 7))
            , new Team("Yellow", new DateTime(2021, 7, 10), new DateTime(2021, 8, 14))
            , new Team("Green", new DateTime(2021, 7, 17), new DateTime(2021, 8, 21))
        );

        /// <summary>
        /// List of match days on which games will be played
        /// </summary>
        public readonly static MatchDay[] MatchDays =
        {
                new MatchDay(new DateTime(2021, 6, 19))
                , new MatchDay(new DateTime(2021, 6, 26))
                , new MatchDay(new DateTime(2021, 7, 3))
                , new MatchDay(new DateTime(2021, 7, 10))
                , new MatchDay(new DateTime(2021, 7, 17))
                , new MatchDay(new DateTime(2021, 7, 24))
                , new MatchDay(new DateTime(2021, 7, 31))
                , new MatchDay(new DateTime(2021, 8, 7))
                , new MatchDay(new DateTime(2021, 8, 14))
                , new MatchDay(new DateTime(2021, 8, 21))
                , new MatchDay(new DateTime(2021, 8, 28))
        };

        public readonly static string[] MatchTimeSlots = { "8:00", "9:30" };

        public Dictionary<string, Matchup> Matchups = null;
        public Dictionary<string, TimeSlots> Slots = null;

        public void GenerateNewSchedule()
        {
            // Clear Matchups and TimeSlots slate
            Matchups = new Dictionary<string, Matchup>();
            Slots = new Dictionary<string, TimeSlots>();

            // Pop a new slate of games into all the MatchDay[] items
            foreach (MatchDay matchDay in MatchDays)
            {
                // Generate the games for this match day
                matchDay.GenerateGames();

                // Add games to list of matchups (team v team) as well as add each time slot per team
                for (int i = 0; i < matchDay.Games.Length; i++)
                {
                    Game game = matchDay.Games[i];

                    // Add the game to Matchups so we make sure no team plays another too few/many times (and also will balance home-away)
                    if (!Matchups.ContainsKey(game.Comparer))
                    {
                        Matchups.Add(game.Comparer, new Matchup(game.Comparer));
                    }
                    Matchups[game.Comparer].AddGame(game);

                    // Add the time slot for each time so we make sure no team plays all games in the same time slot
                    // First do Home Team
                    if (!Slots.ContainsKey(game.HomeTeam.Name))
                    {
                        Slots.Add(game.HomeTeam.Name, new TimeSlots());
                    }
                    Slots[game.HomeTeam.Name].AddSlot(i);
                    // Then do Away Team
                    if (!Slots.ContainsKey(game.AwayTeam.Name))
                    {
                        Slots.Add(game.AwayTeam.Name, new TimeSlots());
                    }
                    Slots[game.AwayTeam.Name].AddSlot(i);
                }
            }
        }

        public bool IsValid
        {
            get
            {
                // No back-to-back rematches
                MatchDay lastMatchDay = null;
                MatchDay secondLastMatchDay = null;
                foreach (MatchDay matchDay in MatchDays)
                {
                    if (lastMatchDay != null)
                    {
                        if (matchDay.Games.Any(g => lastMatchDay.Games.Any(last => g.Comparer == last.Comparer)))
                        {
                            return false;
                        }
                    }
                    if (secondLastMatchDay != null)
                    {
                        if (matchDay.Games.Any(g => secondLastMatchDay.Games.Any(secondlast => g.Comparer == secondlast.Comparer)))
                        {
                            return false;
                        }
                    }

                    secondLastMatchDay = lastMatchDay;
                    lastMatchDay = matchDay;
                }

                // Ensure each team plays each other team balanced number of times, i.e. Game Comparer counts are balanced
                int minComparerCount = int.MaxValue;
                int maxComparerCount = int.MinValue;
                foreach (string matchupComparer in Matchups.Keys)
                {
                    if (Matchups[matchupComparer].MatchupCount > maxComparerCount)
                    {
                        maxComparerCount = Matchups[matchupComparer].MatchupCount;
                    }
                    if (Matchups[matchupComparer].MatchupCount < minComparerCount)
                    {
                        minComparerCount = Matchups[matchupComparer].MatchupCount;
                    }
                    Console.WriteLine($"{matchupComparer}: {Matchups[matchupComparer].MatchupCount}");
                }

                // To be balanced, the minComparerCount and maxComparerCount must be the same, or different by 1
                if (minComparerCount + 1 < maxComparerCount)
                {
                    return false;
                }

                // Ensure that no team has more than MAX
                foreach (TimeSlots timeSlot in Slots.Values)
                {
                    if (!timeSlot.IsValid)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public void BalanceHomeAndAway()
        {
            foreach (Matchup matchup in Matchups.Values)
            {
                for (int i = 1; i < matchup.Games.Count; i++)
                {
                    if (matchup.Games[i].HomeTeam == matchup.Games[i-1].HomeTeam)
                    {
                        // Switch home and away teams
                        Team newHomeTeam = matchup.Games[i].AwayTeam;
                        Team newAwayTeam = matchup.Games[i].HomeTeam;

                        // We've buffered these, so now store
                        matchup.Games[i].HomeTeam = newHomeTeam;
                        matchup.Games[i].AwayTeam = newAwayTeam;
                    }
                }
            }
        }
    }
}
