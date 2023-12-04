using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Command
{
    public class ExportLogEntriesCommand : CommandBase
    {
        private readonly ApiService _apiService;
        public ExportLogEntriesCommand(ApiService apiService)
        {
            _apiService = apiService;
        }
        public override void Execute(object? parameter)
        {
            exportLogEntries(); // IF there's value then put it there. Unless not valid, then provide exception.
        }

        public void exportLogEntries()
        {
            // Combine the directory path with the file name

            String currentDirectory = Directory.GetCurrentDirectory();

            // Navigate up two levels to get the project directory
            String projectDirectory = Directory.GetParent(currentDirectory).Parent.FullName;

            String filePath = Path.Combine(projectDirectory, "logs.txt");

            foreach (KeyValuePair<string, LogEntry> entry in _apiService.LogEntries)
            {
                String _logEntry = $"[{entry.Value.TimeQuery}]{entry.Value.LogNo}. {entry.Key} has {entry.Value.ActionQuery}.";
                File.AppendAllText(filePath, _logEntry + Environment.NewLine);
            }
        }
    }
}
