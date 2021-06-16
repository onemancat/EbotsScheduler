using System;

namespace EbotsScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating season...");
            Generator.GenerateSeason();
            Console.WriteLine("Season generated.");
        }
    }
}
