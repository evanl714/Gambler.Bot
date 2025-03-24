using Microsoft.Extensions.Logging;
using ReactiveUI;
using System.Collections.Generic;

namespace Gambler.Bot.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        
        private MainViewModel _mainViewmodel;

        public MainViewModel MainVM
        {
            get { return _mainViewmodel; }
            set { _mainViewmodel = value; this.RaisePropertyChanged(); }
        }


        public MainWindowViewModel(Microsoft.Extensions.Logging.ILogger<MainWindowViewModel> logger) : base(logger)
        {
            logger.LogDebug("Mainwindow created");
            MainVM = new MainViewModel(logger);
            
        }
    }
}