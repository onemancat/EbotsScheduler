using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EbotsScheduler
{
    class Season
    {
        /// <summary>
        /// List of the teams in the league, and each team's bye week(s)
        /// </summary>
        public readonly static LeagueTeams LeagueTeams = new LeagueTeams(
            new Team("White")
            , new Team("Red")
            , new Team("Yellow")
            , new Team("Green")
            , new Team("Blue")
        );

        public readonly static string[] MatchTimeSlots = { "6:00", "7:30" };

        /// <summary>
        /// List of match days on which games will be played
        /// </summary>
        public readonly static MatchDay[] MatchDays =
        {
            new MatchDay(new DateTime(2021, 9, 11))
            , new MatchDay(new DateTime(2021, 9, 18))
            , new MatchDay(new DateTime(2021, 9, 25))
            , new MatchDay(new DateTime(2021, 10, 2))
            , new MatchDay(new DateTime(2021, 10, 9))
            , new MatchDay(new DateTime(2021, 10, 16))
            , new MatchDay(new DateTime(2021, 10, 23))
            , new MatchDay(new DateTime(2021, 10, 30))
            , new MatchDay(new DateTime(2021, 11, 6))
            , new MatchDay(new DateTime(2021, 11, 13))
            , new MatchDay(new DateTime(2021, 11, 20))
            // No game 11/27/21
            , new MatchDay(new DateTime(2021, 12, 4))
            , new MatchDay(new DateTime(2021, 12, 11))
            , new MatchDay(new DateTime(2021, 12, 18))
        };

        public static MatchDay NextUnfilledMatchDay => MatchDays.FirstOrDefault(m => m.Games == null);

        public readonly static List<Cycle> Cycles = new List<Cycle>();

        /// <summary>
        /// A cyclic schedule generator that produces randomly one "round" of games, and then continues producing that "round", flipping Home/Away
        /// and rotating time slots forward until all MatchDays are done.
        /// </summary>
        public static void GenerateSchedule()
        {
            // Validate that we have the correct number of time slots
            int expectedTimeSlots = ((Season.LeagueTeams.Teams.Length - 2) / 2) + 1;
            if (MatchTimeSlots.Length != expectedTimeSlots)
            {
                throw new Exception($"Expected {expectedTimeSlots} time slots, however {MatchTimeSlots.Length} were defined.");
            }

            // Do the initial cycle
            Cycles.Add(new Cycle().FillMatchdays(null));

            // Do all remaining cycles
            while (NextUnfilledMatchDay != null)
            {
                Cycles.Add(new Cycle().FillMatchdays(Cycles.Last()));
            }

            // If we are here, then we found a valid solution. Swap home/away teams to alternate.
            Console.WriteLine("EBOTS SEASON SCHEDULE");
            Console.WriteLine("==========================================================================================");
            Console.WriteLine($"{Season.LeagueTeams.Teams.Length} Teams");
            Console.WriteLine($"{Season.MatchDays.Length} Match Days");
            Console.WriteLine("==========================================================================================");

            // Print the schedule
            foreach (MatchDay matchDay in Season.MatchDays)
            {
                Console.WriteLine($"{matchDay}");
            }
        }
    }
}
