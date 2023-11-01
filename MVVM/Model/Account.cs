using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Model
{
    public class Account
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

        public Account(string _fn, string _mi, string _ln, string _uid, byte _membership, byte _position, string _institution, string _pn)
        {
            fn = _fn;
            mi = _mi;
            ln = _ln;
            uid = _uid;
            membership = _membership == 1 ? "Professional" : "Student";
            position = _position == 1 ? "Region VIII Officer" : "Member";
            institution = _institution;
            pn = _pn;
        }

        public override string ToString()
        {
            return $"Name: {fn + mi + ln}\nMembership: {membership}\nPosition: {position}\nInstitution: {institution}\nCP No#: {pn}";
        }

    }
}
