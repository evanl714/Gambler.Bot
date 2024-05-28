using Avalonia.Platform.Storage;
using Avalonia.Threading;
using DoormatCore.Sites;
using Gambler.Bot.Classes;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Gambler.Bot.ViewModels.Common
{
    public class RollVerifierViewModel : ViewModelBase
    {
        private Interaction<FilePickerSaveOptions, string> saveFile;
        public Interaction<FilePickerSaveOptions, string> SaveFileInteraction => saveFile;
        public BaseSite Site { get; set; }
        private DoormatCore.Games.Games game;

        public DoormatCore.Games.Games Game
        {
            get { return game; }
            set { game = value; }
        }

        public DoormatCore.Games.Games[] Games
        {
            get { return Site?.SupportedGames; }
        }

        public int StartingNonce { get; set; } = 1;
        public int NumberOfBets { get; set; } = 100;
        public string ServerSeed { get; set; }
        public string ServerSeedHash { get; set; }
        public string ClientSeed { get; set; }

        private ObservableCollection<RollVerifierRoll> rolls;

        public ObservableCollection<RollVerifierRoll> Rolls
        {
            get { return rolls; }
            set { rolls = value; this.RaisePropertyChanged(); }
        }


        private bool canSave;

        public bool CanSave
        {
            get { return canSave; }
            set { canSave = value; this.RaisePropertyChanged(); }
        }



        public RollVerifierViewModel(ILogger logger) : base(logger)
        {
            ConfigureCommands();
        }

        public RollVerifierViewModel(ILogger logger, BaseSite site, DoormatCore.Games.Games game) : base(logger)
        {
            Site = site;
            ConfigureCommands();
            Game = game;
        }
        
        void ConfigureCommands()
        {            
            saveFile = new Interaction<FilePickerSaveOptions, string>();
        }

        private bool isWorking;

        public bool IsWorking
        {
            get { return isWorking; }
            set { isWorking = value; this.RaisePropertyChanged(); }
        }

        private bool hasError;

        public bool HasError
        {
            get { return hasError; }
            set { hasError = value; this.RaisePropertyChanged(); }
        }

        private string error;

        public string Error
        {
            get { return error; }
            set { error = value; this.RaisePropertyChanged(); }
        }


        public async Task Generate()
        {
            IsWorking = true;

            int nonce = StartingNonce;
            int rolls = NumberOfBets;
            int max = nonce + rolls;
            string serverSeed = ServerSeed?.Trim();
            string clientSeed = ClientSeed?.Trim();

            if (string.IsNullOrWhiteSpace(serverSeed) || string.IsNullOrWhiteSpace(clientSeed))
            {
                IsWorking = false;
                HasError = true;
                Error = "Server and client seed required";
                return;
            }

            if (rolls < 1)
            {
                IsWorking = false;
                HasError = true;
                Error = "Number of rolls must be greater than 0";
                return;
            }

            if (nonce < 1)
            {
                IsWorking = false;
                HasError = true;
                Error = "Nonce must be greater than 0";
                return;
            }

            HasError = false;
            Error = null;


            var tmpRolls = new ObservableCollection<RollVerifierRoll>();

            if (!string.IsNullOrWhiteSpace(ServerSeedHash?.Trim()))
            {
                string newHash = Site.GetHash(serverSeed);
                if ( newHash.ToLower()!= ServerSeedHash.Trim().ToLower())
                {
                    HasError = true;
                    Error = "Warning! Server seed does not match provided Hash!";
                }
            }

            
            
            while (nonce< max)
            {
                decimal roll = Site.GetLucky(serverSeed, clientSeed, nonce);
                tmpRolls.Add(new RollVerifierRoll { Roll = roll, Nonce = nonce });
                nonce++;
            }

            Rolls = tmpRolls;
            this.CanSave = true;
            IsWorking = false;
        }

        public async Task Save()
        {

            var result = await SaveFileInteraction.Handle(new FilePickerSaveOptions
            {
                DefaultExtension = ".csv",
                ShowOverwritePrompt = true,
                FileTypeChoices = new List<FilePickerFileType> { new FilePickerFileType("Comma seperated values") { Patterns = new List<string>() { $"*.csv" } } },
                Title = "Save rolls",
                SuggestedFileName = $"LuckyNumbers-{Site?.SiteName}-{DateTime.Today:yyMMdd}.csv"

            });
            if (result == null)
                return;
            try
            {
                IsWorking = true;
                StringBuilder sb = new StringBuilder();
                string delimiter = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
                sb.AppendLine($"Server Seed:{delimiter}{ServerSeed}");
                sb.AppendLine($"Client Seed:{delimiter}{ClientSeed}");
                sb.AppendLine($"");
                sb.AppendLine($"Nonce{delimiter}Roll");
                foreach (var roll in Rolls)
                {
                    sb.AppendLine($"{roll.Nonce}{delimiter}{roll.Roll:0.00000}");
                }
                File.WriteAllText(result, sb.ToString());
            }
            catch (Exception e)
            {
                _logger?.LogError(e.ToString());
            }
            finally
            {
                IsWorking = false;
            }
            //choose a file to save it to with suggested name
            //or show it in a dialog window and then allow saving from there?
        }
    }
}
