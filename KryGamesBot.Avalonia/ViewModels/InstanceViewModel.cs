using DoormatBot;
using ExCSS;
using KryGamesBot.Avalonia.ViewModels.Common;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace KryGamesBot.Avalonia.ViewModels
{
    public class InstanceViewModel
    {
        public SelectSiteViewModel SelectSite { get; set; }
        public bool IsSelectSiteViewVisible { get; set; }
        Doormat botIns = new Doormat();
        public Interaction<LoginViewModel, LoginViewModel?> ShowDialog { get; }


        public InstanceViewModel()
        {
            SelectSite = new SelectSiteViewModel();
            SelectSite.SelectedSiteChanged += SelectSite_SelectedSiteChanged;
            IsSelectSiteViewVisible = true;
            ShowDialog = new Interaction<LoginViewModel, LoginViewModel?>();
        }

        private void SelectSite_SelectedSiteChanged(object? sender, DoormatCore.Helpers.SitesList e)
        {
            SiteChanged(Activator.CreateInstance(e.SiteType()) as DoormatCore.Sites.BaseSite, e.SelectedCurrency?.Name, e.SelectedGame?.Name);
        }

        async Task ShowLogin()
        {
            var store = new LoginViewModel();

            var result = await ShowDialog.Handle(store);
        }

        void SiteChanged(DoormatCore.Sites.BaseSite NewSite, string currency, string game)
        {
            botIns.CurrentSite = NewSite;
            if (currency != null && Array.IndexOf(botIns.CurrentSite.Currencies, currency) >= 0)
                botIns.CurrentSite.Currency = Array.IndexOf(botIns.CurrentSite.Currencies, currency);
            object curGame = DoormatCore.Games.Games.Dice;
            if (game != null && Enum.TryParse(typeof(DoormatCore.Games.Games), game, out curGame) && Array.IndexOf(botIns.CurrentSite.SupportedGames, (DoormatCore.Games.Games)curGame) >= 0)
                botIns.CurrentGame = (DoormatCore.Games.Games)curGame;
            ShowLogin().Wait();
            /*LoginControl.CurrentSite = botIns.CurrentSite;
            lciSelectSite1.Visibility = Visibility.Collapsed;
            lciLoginControl.Visibility = Visibility.Visible;
            itmCurrency.Items.Clear();
            foreach (string x in botIns.CurrentSite.Currencies)
            {
                var itm = new BarCheckItem();
                itm.Content = x;
                itm.CheckedChanged += Itm_CheckedChanged;
                itmCurrency.Items.Add(itm);
            }
            itmGame.Items.Clear();
            foreach (var x in botIns.CurrentSite.SupportedGames)
            {
                var itm = new BarCheckItem();
                itm.Content = x.ToString();
                itm.CheckedChanged += Itm_CheckedChanged;
                itmGame.Items.Add(itm);
            }
            lueCurrencies.ItemsSource = botIns.CurrentSite.Currencies;
            lueCurrencies.EditValue = botIns.CurrentSite.CurrentCurrency;
            lueGames.ItemsSource = botIns.CurrentSite.SupportedGames;
            lueGames.EditValue = botIns.CurrentGame;
            Rename?.Invoke(this, new RenameEventArgs { newName = "Log in - " + NewSite?.SiteName });*/
        }
    }
}
