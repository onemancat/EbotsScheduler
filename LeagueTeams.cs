using System;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class LeagueTeams
    {
        public Team[] Teams { get; set; }

        public LeagueTeams(params Team[] teams)
        {
            Teams = teams;
        }

        public bool IsOddNumberOfTeams => Teams.Length % 2 == 1;

        /// <summary>
        /// The number of weeks in a cycle. We will randomly create a single cycle, and then deterministically fill out (copy, with rotation modifications)
        /// the rest of the schedule based on the initial randomly generated cycle.
        /// </summary>
        public int MatchDaysPerCycle
        {
            get
            {
                if (IsOddNumberOfTeams)
                {
                    // Odd # of teams, the cycle is #teams
                    return Teams.Length;
                }
                else
                {
                    // Even # of teams, the cycle is #teams - 1
                    return Teams.Length - 1;
                }
            }
        }
    }
}
