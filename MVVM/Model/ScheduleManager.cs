using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Model
{
    public class ScheduleManager
    {
        public static Dictionary<string, DayContent> GetDayController(Attendee _profile)
        {
            return new Dictionary<string, DayContent>()
            {
                ["12/02"] = _profile.Day1,
                ["12/03"] = _profile.Day2,
                ["12/04"] = _profile.Day3,
            };
        }

        public static Dictionary<string, (TimeSpan from, TimeSpan to, String columnPrefix, String radioButtonPrefix)> GetTimeController()
        {
            return new()
            {
                //{ "AmSnack", (new TimeSpan(8, 0, 0), new TimeSpan(11, 59, 0), "am", "AM") }, // Morning Snack (Between 8 AM - 11:59 AM)
                //{ "LunchSnack", (new TimeSpan(12, 0, 0), new TimeSpan(15, 59, 59), "lunch" ,"L") }, // Lunch Snack (Between 12 PM - 3:59 PM)
                //{ "PmSnack", (new TimeSpan(16, 0, 0), new TimeSpan(17, 59, 59), "pm" , "PM") }, // Evening Snack (Between 4 PM - 5:59 PM)
                { "CheckOut", (new TimeSpan(17, 0, 0), new TimeSpan(23, 0, 0), "checkout" , "CheckOut") }, // Check Out (Time Out) (Between 5 PM - 11:00 PM)
            };
        }

        public static string[] GetDayDBColumnName(byte DayCount = 0, bool combined = false)
        {
            string[][] TimeTableDBColumnNames =
            {
                new string[] { "amd1", "lunchd1", "pmd1", "checkind1", "checkoutd1" },
                new string[] { "amd2", "lunchd2", "pmd2", "checkind2", "checkoutd2" },
                new string[] { "amd3", "lunchd3", "pmd3", "checkind3", "checkoutd3" }
            };
            Dictionary<byte, string[]> DayDBContent = new() {
                [1] = TimeTableDBColumnNames[0],
                [2] = TimeTableDBColumnNames[1],
                [3] = TimeTableDBColumnNames[2] 
            };

            if(combined)
            {
                return TimeTableDBColumnNames.SelectMany(x => x).ToArray();
            }

            if(DayDBContent.TryGetValue(DayCount, out var Result))
            {
                return Result;
            } else
            {
                throw new Exception("No Key found.");
            }
        }
    }
}
