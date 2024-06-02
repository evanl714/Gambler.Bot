using Gambler.Bot.Strategies.Strategies.Abstractions;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.Common
{
    public class ConsoleViewModel: ViewModelBase
    {
        private IProgrammerMode strat;

        public IProgrammerMode Strategy
        {
            get { return strat; }
            set { strat = value; this.RaisePropertyChanged(nameof(IsEnabled)); }
        }
        public bool IsEnabled { get => Strategy != null; }

        private ObservableCollection<string> lines;

        public ObservableCollection<string> Lines
        {
            get { return lines; }
            set { lines = value; this.RaisePropertyChanged(); }
        }
        private string command;

        public string Command
        {
            get { return command; }
            set { command = value; this.RaisePropertyChanged(); }
        }

        private ObservableCollection<string> previousCommands = new ObservableCollection<string>();

        public ObservableCollection<string> PreviousCommands
        {
            get { return previousCommands; }
            set { previousCommands = value; }
        }

        private int searchIndex;

        public int SearchIndex
        {
            get { return searchIndex; }
            set { searchIndex = value; }
        }

        public ConsoleViewModel(ILogger logger):base(logger)
        {
            
        }

        private int selectedIndex=-1;

        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; this.RaisePropertyChanged(); }
        }


        public void AddLine(string line)
        {
            if (Lines == null)
                Lines = new ObservableCollection<string>();
            Lines.Add(line);
            while (Lines.Count>1000)
            {
                Lines.RemoveAt(0);

            }
            SelectedIndex = Lines.Count - 1;
        }
        
        public void ExecuteCommand()
        {
            if (Strategy == null)
                return;
            AddLine(">> " + Command);
            PreviousCommands.Add(Command);
            SearchIndex = PreviousCommands.Count;
            try
            {
                Strategy.ExecuteCommand(Command);
            }
            catch (Exception e)
            {
                AddLine(e.ToString());
            }
            Command = "";
            while (PreviousCommands.Count>100)
            {
                PreviousCommands.RemoveAt(0);
            }
        }

        public void NavigateCommand(bool Up)
        {
            if (Up)
                SearchIndex--;
            else
                SearchIndex++;
            if (searchIndex<0)
                searchIndex = 0;
            if (searchIndex>=PreviousCommands.Count)
                searchIndex = PreviousCommands.Count - 1;
            if (PreviousCommands.Count>0)
                Command = PreviousCommands[searchIndex];

        }
    }
}
