using System;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class Generator
    {
        public static void GenerateSeason()
        {
            Season season = new Season();

            // For each match day, randomly create 2 Games until season is valid (satisfies all constraints in Season.IsValid)
            int tries = 0;
            do
            {
                season.GenerateNewSchedule();

                if (++tries % 10 == 0)
                {
                    Console.WriteLine($"Tried solution #{tries}...");
                }
            } while (tries < Season.MAX_TRIES && !season.IsValid);
            
            if (season.IsValid)
            {
                Console.WriteLine($"Found valid solution after {tries} tries, balancing Home-Away for matchups...");

                // If we are here, then we found a valid solution. Swap home/away teams to alternate.
                season.BalanceHomeAndAway();

                Console.WriteLine("SEASON SCHEDULE");
                Console.WriteLine("==========================================================================================");
                Console.WriteLine("Constraints");
                Console.WriteLine("-----------");
                Console.WriteLine($"{Season.LeagueTeams.Teams.Length} Teams");
                Console.WriteLine($"{Season.MatchDays.Length} Match Days");
                Console.WriteLine($"{Season.MatchTimeSlots.Length} Time Slots");
                Console.WriteLine("All teams play each other as close to the same number of times as is feasible");
                Console.WriteLine("Teams shall not rematch on either of the next 2 match days");
                Console.WriteLine("Rematches between teams shall alternate home and away");
                Console.WriteLine("Pre-determined Bye Weeks are observed");
                Console.WriteLine($"No team shall play more than {Season.MAX_TIMESLOT_PERCENT}% of its games in any time slot");
                Console.WriteLine("==========================================================================================");

                // Print the schedule
                foreach (MatchDay matchDay in Season.MatchDays)
                {
                    Console.WriteLine($"{matchDay}");
                }
            }
            else
            {
                Console.WriteLine($"Could not find valid solution after {tries} tries.");
            }
        }
    }
}
