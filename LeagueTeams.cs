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
    }
}
