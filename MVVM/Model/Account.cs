using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Model
{
    public class Attendee
    {
        public string UID { get; set; }
        public string FN { get; set; }
        public string MI { get; set; }
        public string LN { get; set; }
        public string Membership { get; set; }
        public byte MembershipBit { get; set; }
        public string Position { get; set; }
        public byte PositionBit { get; set; }
        public string Institution { get; set; }
        public string PN { get; set; }
        public DayContent Day1 { get; set; }
        public DayContent Day2 { get; set; }
        public DayContent Day3 { get; set; }

        public Attendee(string _fn, string _mi, string _ln, string _uid, byte _membership, byte _position, string _institution, string _pn, DayContent _day1, DayContent _day2, DayContent _day3)
        {
            FN = _fn;
            MI = _mi;
            LN = _ln;
            UID = _uid;
            Membership = _membership == 1 ? "Professional" : "Student";
            Position = _position == 1 ? "Region VIII Officer" : "Member";
            Institution = _institution;
            PN = _pn;
            Day1 = _day1;
            Day2 = _day2;
            Day3 = _day3;
        }

        public Attendee Clone()
        {
            return new Attendee(FN, MI, LN, UID, MembershipBit, PositionBit, Institution, PN, Day1, Day2, Day3);
        }

        public override string ToString()
        {
            return $"Name: {FN + MI + LN}\nMembership: {Membership}\nPosition: {Position}\nInstitution: {Institution}\nCP No#: {PN}";
        }

    }
}
