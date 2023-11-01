
using System.Collections.Generic;

namespace QEAMApp.MVVM.Model
{
    public class Attendee
    {
        public Account Profile { get; set; }
        public Attendee(Account account)
        {
            Profile = account;
        }
    }
}
