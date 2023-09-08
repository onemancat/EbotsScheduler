using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EbotsScheduler
{
    class Season
    {
        /// <summary>
        /// List of the teams in the league, and each team's requested bye week(s).
        /// The team names must be entered into TeamSnap exactly as listed here prior to schedule import.
        /// </summary>
        public readonly static LeagueTeams LeagueTeams = new LeagueTeams(
            new Team("White")
            , new Team("Red")
            , new Team("Green")
            , new Team("Blue")
            , new Team("Yellow")
            , new Team("Black")
            , new Team("Orange")
        );

        /// <summary>
        /// This division must be loaded into TeamSnap prior to import: My Organizations > EBOTS (current season) > 
        /// Rostering > Division / Team
        /// </summary>
        public readonly static string DivisionNameInTeamSnap = "East Bay Over 30 Soccer";

        /// <summary>
        /// This location must be loaded into TeamSnap prior to import: My Organizations > EBOTS (current season) >
        /// Schedule > Locations > Copy Locations from Another League > select bottom-most (most recent) archived
        /// </summary>
        public readonly static string LocationNameInTeamSnap = "Estuary Park (Alameda)";

        // This format is required for TeamSnap upload, do not change
        public readonly static string[] MatchTimeSlots = { "08:00 AM", "09:30 AM", "11:00 AM" };

        /// <summary>
        /// List of match days on which games will be played
        /// </summary>
        public readonly static MatchDay[] MatchDays =
        {
            // With 7 teams it is really nice to have multiple of 7 match days so we get a full cycle
            new MatchDay(new DateTime(2023, 9, 9))
            , new MatchDay(new DateTime(2023, 9, 16))
            , new MatchDay(new DateTime(2023, 9, 23))
            , new MatchDay(new DateTime(2023, 9, 30))
            , new MatchDay(new DateTime(2023, 10, 7))
            , new MatchDay(new DateTime(2023, 10, 14))
            , new MatchDay(new DateTime(2023, 10, 21))
            , new MatchDay(new DateTime(2023, 10, 28))
            , new MatchDay(new DateTime(2023, 11, 4))
            , new MatchDay(new DateTime(2023, 11, 11))
            , new MatchDay(new DateTime(2023, 11, 18))
            , new MatchDay(new DateTime(2023, 12, 2))
            , new MatchDay(new DateTime(2023, 12, 9))
            , new MatchDay(new DateTime(2023, 12, 16))
        };

        public static MatchDay NextUnfilledMatchDay => MatchDays.FirstOrDefault(m => m.Games == null);

        public  static List<Cycle> Cycles = null;

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

            int iterations = 0;
            bool isFeasible;
            do
            {
                // Start from scratch each iteration
                Cycles = new List<Cycle>();

                // Do the initial cycle
                Cycle validInitialCycle = null;
                while (validInitialCycle == null)
                {
                    validInitialCycle = new Cycle().FillMatchdays(null);
                }
                Cycles.Add(new Cycle().FillMatchdays(null));

                // Do all remaining cycles
                while (NextUnfilledMatchDay != null)
                {
                    Cycles.Add(new Cycle().FillMatchdays(Cycles.Last()));
                }

                isFeasible = DoesScheduleSatisfyAllMustHaveByeDates();

            } while (!isFeasible && ++iterations <= 10000);

            // If we are here, then we found a valid solution. Swap home/away teams to alternate.
            Console.WriteLine("EBOTS SEASON SCHEDULE");
            Console.WriteLine("==========================================================================================");
            if (!isFeasible)
            {
                // Since we create a cycle and then copy it, it is certainly possible that not all constraints can be honored
                // In this case, we should manually swap matchdays around
                Console.WriteLine("WARNING: Constaints could not all be satisfied. Manual schedule adjustment required.");
                Console.WriteLine("==========================================================================================");
            }
            Console.WriteLine($"{Season.LeagueTeams.Teams.Length} Teams");
            Console.WriteLine($"{Season.MatchDays.Length} Match Days");
            Console.WriteLine("==========================================================================================");

            // Print the schedule
            foreach (MatchDay matchDay in Season.MatchDays)
            {
                Console.WriteLine($"{matchDay}");
            }

            // Produce the TeamSnap import file
            StringBuilder csv = new StringBuilder();
            csv.AppendLine("\"Date\",\"Start Time\",\"Division\",\"Home Team\",\"Away Team\",\"Location\"");
            foreach (MatchDay matchDay in Season.MatchDays)
            {
                for (int i = 0; i < matchDay.Games.Length; i++)
                {
                    Game game = matchDay.Games[i];
                    csv.AppendLine($"\"{matchDay.Date:MM/dd/yyyy}\",\"{Season.MatchTimeSlots[i]}\",\"{DivisionNameInTeamSnap}\",\"{game.HomeTeam.Name}\",\"{game.AwayTeam.Name}\",\"{LocationNameInTeamSnap}\"");
                }
            }
            string outputPath = @$"C:\Systems\EbotsScheduler\Schedule-{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.csv";
            System.IO.File.WriteAllText(outputPath, csv.ToString());
        }

        public static bool DoesScheduleSatisfyAllMustHaveByeDates()
        {
            // Check to make sure required byes are satified
            foreach (Team team in LeagueTeams.Teams)
            {
                foreach (DateTime mustHaveByeDate in team.MustHaveByeDates)
                {
                    foreach (MatchDay matchDay in Season.MatchDays)
                    {
                        if (matchDay.Date == mustHaveByeDate)
                        {
                            foreach (Game game in matchDay.Games)
                            {
                                if (game.HomeTeam.Name == team.Name || game.AwayTeam.Name == team.Name)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }

    }
}
