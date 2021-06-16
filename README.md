EbotsScheduler

Simple program to create a balanced league schedule for a sports league, given a certain number of teams, number of match days, number of time slots, and pre-existing bye-week constraints.

Scheduler will use brute force to create random schedules until all constraints are satisfied.

Current constraints include:
- All teams play each other as close to the same number of times as is feasible
- Teams shall not rematch on either of the next 2 match days
- Rematches between teams shall alternate home and away
- Pre-determined Bye Weeks are observed
- No team shall play more than X% of its games in any time slot