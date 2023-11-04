using System;
using System.Collections.Generic;

namespace QEAMApp.MVVM.Model
{
    public class DayContent
    {
        public DateTime? amSnack { get; set; }
        public DateTime? lunchSnack { get; set; }
        public DateTime? pmSnack { get; set; }
        public DateTime? checkIn { get; set; }
        public DateTime? checkOut { get; set; }

        public DayContent()
        {
            this.amSnack = null;
            this.lunchSnack = null;
            this.pmSnack = null;
            this.checkIn = null;
            this.checkOut = null;
        }

        public DayContent(Dictionary<string, object> values, string[] key)
        {
            this.amSnack = GetNullableDateTimeValue(values, key[0]);
            this.lunchSnack = GetNullableDateTimeValue(values, key[1]);
            this.pmSnack = GetNullableDateTimeValue(values, key[2]);
            this.checkIn = GetNullableDateTimeValue(values, key[3]);
            this.checkOut = GetNullableDateTimeValue(values, key[4]);
        }

        // Replace DateTime with String, but still read as DateTime
        private DateTime? GetNullableDateTimeValue(Dictionary<string, object> values, string key)
        {
            if (values.TryGetValue(key, out object? value) && DateTime.TryParse(value?.ToString(), out DateTime result))
            {
                return result;
            }
            return null;
        }
    }
}
