using System;
using System.Collections.Generic;
using System.Text;

namespace EbotsScheduler
{
    /// <summary>
    /// Stores for a given team the number of games scheduled in each time slot
    /// </summary>
    class TimeSlots
    {
        // No need to initialize since in .NET default value is zero
        public int[] slotCounts = new int[Season.MatchTimeSlots.Length];
        public int totalSlots = 0;

        public void AddSlot(int slotNumber)
        {
            slotCounts[slotNumber]++;
            totalSlots++;
        }

        public bool IsValid
        {
            get
            {
                for (int i = 0; i < Season.MatchTimeSlots.Length; i++)
                {
                    if ((slotCounts[i] * 100 / totalSlots) > Season.MAX_TIMESLOT_PERCENT)
                    {
                        return false;
                    }
                }
                
                return true;
            }
        }
    }
}
