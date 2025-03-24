using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Svg;
using Gambler.Bot.Core.Helpers;
using Microsoft.Extensions.Logging;
using ReactiveUI;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace Gambler.Bot.ViewModels.Common
{
    public class SelectSiteViewModel:ViewModelBase
    {
        private ObservableCollection<AvaSitesList> _sites = new ObservableCollection<AvaSitesList>();
        public ObservableCollection<AvaSitesList> Sites { get=> _sites; set => this.RaiseAndSetIfChanged(ref _sites, value); }
        public string Text { get; set; }

        public List<string> strings { get; set; } = new List<string> { "1", "2", "three", "fuck", "you" };

        public event EventHandler<SitesList> SelectedSiteChanged;
        public bool BypassLogIn { get; set; } = false;

        public SelectSiteViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            logger.LogDebug("SelectSite Creating");
            Classes.AutoBet botIns = new Classes.AutoBet(_logger);
            botIns.CompileSites();
            botIns.GetStrats();
            LoginCommand = ReactiveCommand.Create<object>(LogIn);
            SimulateCommand = ReactiveCommand.Create<object>(Simulate);
            ViewSiteCommand = ReactiveCommand.Create<object>(ViewSite);
            Sites = new ObservableCollection<AvaSitesList>(Classes.AutoBet.Sites.Select(x=>new AvaSitesList(x) ));
            logger.LogDebug("SelectSite Created");
        }


        public ICommand LoginCommand { get; }

        void LogIn(object site)
        {
            if (site is AvaSitesList Site)
            {
                BypassLogIn = false;
                SelectedSiteChanged.Invoke(this, Site.Site);
            }
        }
        public ICommand SimulateCommand { get; }

        void Simulate(object site)
        {
            if (site is AvaSitesList Site)
            {
                BypassLogIn = true;
                SelectedSiteChanged.Invoke(this, Site.Site);
            }
        }
        public ICommand ViewSiteCommand { get; }

        void ViewSite(object site)
        {
            if (site is AvaSitesList Site)
            {
                Process.Start( new ProcessStartInfo { FileName= Site.Site.URL, UseShellExecute = true });
            }
        }
    }

    public class AvaSitesList
    {
        public AvaSitesList(SitesList site)
        {
            Site = site;
            image = new Bitmap(AssetLoader.Open(new Uri($"avares://Gambler.Bot/Assets/Images/Sites/{Site.Name.ToLower()}.png")));
            Currencies = site.Currencies.Select(x => new AvaCurrency (x)).ToList();
            Games = site.SupportedGames.Select(x => new AvaGame(x.ToString())).ToList();
        }
    
        public SitesList Site { get; set; }
        
        private Bitmap image;

        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }
        public List<AvaCurrency> Currencies { get; set; }
        public List<AvaGame> Games { get; set; }
        public string SelectString { get => "Bet @ " + Site.Name; }

    }
    public class AvaCurrency
    {
        public AvaCurrency(string currency)
        {
            Currency = currency;

            var tmpImage = new SKSvg();
            Uri image = new Uri($"avares://Gambler.Bot/Assets/Images/Currencies/{Currency.ToLower()}.svg");
            if (AssetLoader.Exists(image))
            {
                var asset = AssetLoader.Open(image);
                tmpImage.Load(asset);
                Svg = new SvgImage();
                Svg.Source = new SvgSource();
                Svg.Source.Picture = tmpImage.Model;
            }
            
        }
        
    
        public string Currency { get; set; }
        public string? Img 
        { 
            get; 
            set; 
        }

        SvgImage Svg;
        public Image Image
        {
            get => Svg != null ? new Image { Source = Svg } : null;
        }
    }
    public class AvaGame
    {
        public AvaGame(string game)
        {
            Game = game;
            //Image = $"Assets/Images/Games/{Game.ToLower()}.svg";

            var tmpImage = new SKSvg();
            Uri image = new Uri($"avares://Gambler.Bot/Assets/Images/Games/{Game.ToLower()}.svg");
            if (AssetLoader.Exists(image))
            {
                var asset = AssetLoader.Open(image);
                tmpImage.Load(asset);
                Svg = new SvgImage();
                Svg.Source = new SvgSource();
                Svg.Source.Picture = tmpImage.Model;
            }            
        }

        SvgImage Svg; 
        public string Game { get; set; }
        public Image Image
        {
            get => Svg != null ? new Image { Source = Svg } : null;            
        }
    }
}
