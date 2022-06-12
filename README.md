EbotsScheduler

Simple program to create a balanced league schedule for a sports league, given a certain number of teams, match days, and time slots

Scheduler will randomly create an initial cycle of games, and then rotate home/away and time slots for additional cycles until all match days have been filled

To use, first edit Season.cs to input teams, match days, and bye week requests, and then run the program.

Check for any WARNING if the solution is infeasible. This can happen if a bye week request breaks the cyclicality of the second or subsequent cycle.

The program will produce a TeamSnap import file. Prepare TeamSnap with location and teams per https://helpme.teamsnap.com/article/1292-import-schedules#org-schedule-import

When done, import the schedule into TeamSnap and all is done!
