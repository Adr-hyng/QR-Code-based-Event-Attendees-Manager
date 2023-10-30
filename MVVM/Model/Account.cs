using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Model
{
    public class Account
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string MembershipType { get; set; } // Get this through Boolean manip. like 1 = Professional, 0 = Member from DB.
        public string Position { get; set; }
        public string Institution { get; set; }
        public string PhoneNumber { get; set; }

        public Account(string name, string id, bool membershipType, bool position, string institution, string phoneNumber)
        {
            Name = name;
            ID = id;
            MembershipType = membershipType ? "Professional" : "Student";
            Position = position ? "Region VIII Officer" : "Member";
            Institution = institution;
            PhoneNumber = phoneNumber;
        }

        public bool Authentication()
        {
            return false;
        }
    }
}
