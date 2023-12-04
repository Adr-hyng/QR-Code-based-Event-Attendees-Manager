using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Model
{
    public class LogEntry
    {
        public DateTime? TimeQuery;
        public int LogNo;
        public String Name;
        public String ActionQuery;

        public LogEntry(DateTime timeQuery, int logNo, String name, String actionQuery)
        {
            TimeQuery = timeQuery;
            LogNo = logNo;
            Name = name;
            ActionQuery = actionQuery;
        }

        public LogEntry(int logNo, String name, String actionQuery)
        {
            TimeQuery = DateTime.Now;
            LogNo = logNo;
            Name = name;
            ActionQuery = actionQuery;
        }
    }
}
