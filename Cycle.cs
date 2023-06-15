using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class Cycle
    {
        public static Random Randomizer = new Random();

        private List<MatchDay> _cycleMatchDays = new List<MatchDay>();
        public MatchDay[] MatchDays => _cycleMatchDays.ToArray();

        public Cycle FillMatchdays(Cycle rotateFromPreviousCycle)
        {
            // If this is initial cycle, clear all games
            if (rotateFromPreviousCycle == null)
            {
                foreach (MatchDay matchDay in Season.MatchDays)
                {
                    matchDay.ClearGames();
                }
            }

            // Set a random bye team order for the cycle
            List<Team> byeSequence = null;
            if(Season.LeagueTeams.Teams.Length % 2 == 1)
            {
                byeSequence = Season.LeagueTeams.Teams.OrderBy(t => Randomizer.Next()).ToList();
            }

            // Generate the initial cycle
            for (int cycleMatchDayNumber = 0; cycleMatchDayNumber < Season.LeagueTeams.MatchDaysPerCycle; cycleMatchDayNumber++)
            {
                // Work on the next unfilled MatchDay
                MatchDay thisMatchDay = Season.NextUnfilledMatchDay;

                if (thisMatchDay != null)
                {
                    // Add this matchDay to the cycle
                    _cycleMatchDays.Add(thisMatchDay);

                    if (rotateFromPreviousCycle != null)
                    {
                        // Copy the analogously sequential MatchDay from the previous cycle, with rotation
                        MatchDay previousCycleMatchDay = rotateFromPreviousCycle.MatchDays[cycleMatchDayNumber];
                        
                        // Set up array to hold this Match Day games
                        Game[] thisMatchDayGames = new Game[previousCycleMatchDay.Games.Length];

                        for (int gameNumber = 0; gameNumber < previousCycleMatchDay.Games.Length; gameNumber++)
                        {
                            // Copy each game in, but place in 1 index higher (for slot rotation), and switch home and away
                            Game thisGame = new Game(previousCycleMatchDay.Games[gameNumber].AwayTeam, previousCycleMatchDay.Games[gameNumber].HomeTeam);
                            int thisGameSlotIndex = gameNumber + 1;
                            if (thisGameSlotIndex >= previousCycleMatchDay.Games.Length)
                            {
                                thisGameSlotIndex = 0;
                            }
                            thisMatchDayGames[thisGameSlotIndex] = thisGame;
                        }

                        thisMatchDay.Games = thisMatchDayGames;
                    }
                    else
                    {
                        // Generate a new random set of games for this MatchDay

                        // Determine the bye team, if there is one
                        Team byeTeam = null;
                        if (byeSequence != null)
                        {
                            byeTeam = byeSequence[cycleMatchDayNumber];
                        }

                        // Find all the teams which do not have a bye this MatchDay
                        Team[] teamsPlayingThisMatchDay = Season.LeagueTeams.Teams.Where(t => byeTeam == null || t.Name != byeTeam.Name).ToArray();

                        // The only constraint is that within a cycle, no games can be repeated
                        int redundantTries = 0;
                        while (true)
                        {
                            thisMatchDay.GenerateGames(teamsPlayingThisMatchDay);
                            if (ContainsRedundantGames)
                            {
                                if (++redundantTries >= 5)
                                {
                                    // Blow up the cycle
                                    return null;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            return this;
        }


        public bool ContainsRedundantGames
        {
            get
            {
                SortedSet<string> matchComparers = new SortedSet<string>();
                for (int cycleMatchDayNumber = 0; cycleMatchDayNumber < _cycleMatchDays.Count; cycleMatchDayNumber++)
                {
                    MatchDay cycleMatchDay = _cycleMatchDays[cycleMatchDayNumber];
                    if (cycleMatchDay.Games != null)
                    {
                        foreach (Game game in cycleMatchDay.Games)
                        {
                            if (matchComparers.Contains(game.Comparer))
                            {
                                return true;
                            }
                            else
                            {
                                matchComparers.Add(game.Comparer);
                            }
                        }
                    }
                }
                return false;
            }
        }

    }
}
