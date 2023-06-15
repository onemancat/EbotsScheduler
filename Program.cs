using System;

namespace EbotsScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("In TeamSnap:");
            Console.WriteLine();
            Console.WriteLine($" (1) Have you added the location {Season.LocationNameInTeamSnap} into TeamSnap?");
            Console.WriteLine($" (2) Do the team names in TeamSnap match exactly those in the Season class in this application?");
            Console.WriteLine($" (3) Are the teams in the TeamSnap division {Season.DivisionNameInTeamSnap}?");
            Console.WriteLine();
            Console.WriteLine("You can check the division using the filters on the Registration tab.");
            Console.WriteLine();
            Console.Write("Press 'Y' if this is all correct. Press any other key to exit: ");

            if (Console.ReadKey().Key.ToString().ToUpper() == "Y")
            {
                Season.GenerateSchedule();
            }
        }
    }
}
