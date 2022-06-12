using System;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class Team
    {
        public string Name { get; set; }
        public DateTime[] MustHaveByeDates { get; set; }

        public Team(string name, params DateTime[] mustHaveByeDates)
        {
            Name = name;
            MustHaveByeDates = mustHaveByeDates;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
