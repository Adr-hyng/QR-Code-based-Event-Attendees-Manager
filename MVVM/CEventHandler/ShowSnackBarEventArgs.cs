using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.CEventHandler
{
    public class ShowSnackBarEventArgs : EventArgs
    {
        public double Duration { get; set; }

        public ShowSnackBarEventArgs(double duration)
        {
            Duration = duration;
        }
    }

}
