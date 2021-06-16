using System;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    class Team
    {
        public string Name { get; set; }
        public DateTime[] ByeWeeks { get; set; }

        public Team(string name, params DateTime[] byeWeeks)
        {
            Name = name;
            ByeWeeks = byeWeeks;
        }
    }
}
