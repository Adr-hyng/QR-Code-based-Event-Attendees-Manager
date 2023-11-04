using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace QEAMApp.MVVM.Model
{
    public class DayContent
    {
        public string id;
        public DateTime? AmSnack { get; set; }
        public DateTime? LunchSnack { get; set; }
        public DateTime? PmSnack { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        public DayContent()
        {
            this.AmSnack = null;
            this.LunchSnack = null;
            this.PmSnack = null;
            this.CheckIn = null;
            this.CheckOut = null;
        }

        public DayContent(Dictionary<string, object> values, string[] key)
        {
            Regex dayIdentifier = new("d[1-3]");
            this.id = dayIdentifier.Match(key[0]).Value;
            this.AmSnack = GetNullableDateTimeValue(values, key[0]);
            this.LunchSnack = GetNullableDateTimeValue(values, key[1]);
            this.PmSnack = GetNullableDateTimeValue(values, key[2]);
            this.CheckIn = GetNullableDateTimeValue(values, key[3]);
            this.CheckOut = GetNullableDateTimeValue(values, key[4]);
        }

        // Replace DateTime with String, but still read as DateTime
        private static DateTime? GetNullableDateTimeValue(Dictionary<string, object> values, string key)
        {
            if (values.TryGetValue(key, out object? value) && DateTime.TryParse(value?.ToString(), out DateTime result))
            {
                return result;
            }
            return null;
        }

        public bool InTimeBound(DateTime time, TimeSpan startTime, TimeSpan endTime)
        {
            TimeSpan currentTime = time.TimeOfDay;

            if (endTime < startTime)
            {
                // If the end time is before the start time, it means the range spans across midnight
                // In this case, we need to check if the current time is after the start time or before the end time of the next day
                return currentTime >= startTime || currentTime <= endTime.Add(new TimeSpan(1, 0, 0));
            }
            else if (endTime == startTime)
            {
                // If the end time is equal to the start time, it means the range is exactly one minute
                // In this case, we need to check if the current time is equal to the start time
                return currentTime == startTime;
            }
            else if (currentTime < startTime)
            {
                // If the current time is before the start time, it is not within the range
                return false;
            }
            else if (currentTime > endTime)
            {
                // If the current time is after the end time, it is not within the range
                return false;
            }
            else
            {
                // If the current time is between the start time and end time, it is within the range
                return true;
            }
        }
    }
}
