using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QEAMApp.MVVM.Command
{
    internal class ExportLogEntriesCommand : CommandBase
    {
        private readonly ControlScreenViewModel _controlViewModel;
        public ExportLogEntriesCommand(ControlScreenViewModel controlViewModel)
        {
            _controlViewModel = controlViewModel;
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
            String projectDirectory = Directory.GetParent(currentDirectory)!.Parent!.FullName;

            String filePath = Path.Combine(projectDirectory.Replace("\\bin", "\\Database"), "logs.txt");

            foreach (KeyValuePair<string, LogEntry> entry in _controlViewModel._apiService.LogEntries)
            {
                String _logEntry = $"[{entry.Value.TimeQuery}]{entry.Value.Name} has {entry.Value.ActionQuery}.";
                File.AppendAllText(filePath, _logEntry + Environment.NewLine);
            }
            
        }
    }
}
