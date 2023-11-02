using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Model
{
    public class Attendee
    {
        public string uid { get; set; }
        public string fn { get; set; }
        public string mi { get; set; }
        public string ln { get; set; }
        public string membership { get; set; }
        public byte membershipBit { get; set; }
        public string position { get; set; }
        public byte positionBit { get; set; }
        public string institution { get; set; }
        public string pn { get; set; }
        public DayContent day1 { get; set; }
        public DayContent day2 { get; set; }
        public DayContent day3 { get; set; }

        public Attendee(string _fn, string _mi, string _ln, string _uid, byte _membership, byte _position, string _institution, string _pn, DayContent _day1, DayContent _day2, DayContent _day3)
        {
            fn = _fn;
            mi = _mi;
            ln = _ln;
            uid = _uid;
            membership = _membership == 1 ? "Professional" : "Student";
            position = _position == 1 ? "Region VIII Officer" : "Member";
            institution = _institution;
            pn = _pn;
            day1 = _day1;
            day2 = _day2;
            day3 = _day3;
        }

        public Attendee Clone()
        {
            return new Attendee(fn, mi, ln, uid, membershipBit, positionBit, institution, pn, day1, day2, day3);
        }

        public override string ToString()
        {
            return $"Name: {fn + mi + ln}\nMembership: {membership}\nPosition: {position}\nInstitution: {institution}\nCP No#: {pn}";
        }

    }
}
