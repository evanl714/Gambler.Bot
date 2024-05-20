using KryGamesBot.Ava.Classes;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels
{
    public class MainViewModel:ViewModelBase
    {
        private InstanceViewModel _instance;
        public InstanceViewModel Instance { get =>_instance; set { _instance = value; this.RaisePropertyChanged(); } }
        public MainViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            string path = string.Empty;
            UISettings.Portable = File.Exists("portable"); ;
            if (UISettings.Portable)
                path = "";
            else
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "KryGamesBot");
            }
            Instance = new InstanceViewModel(logger);
            if (File.Exists(Path.Combine(path, "UISettings.json")))
            {
                UISettings.Settings = JsonSerializer.Deserialize<UISettings>(File.ReadAllText(Path.Combine(path, "UISettings.json")));
                //change the theme somehow?
            }
        }
    }
}
