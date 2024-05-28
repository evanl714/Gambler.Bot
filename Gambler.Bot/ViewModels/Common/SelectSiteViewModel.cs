using ActiproSoftware.UI.Avalonia.Media;
using Avalonia;
using Avalonia.Controls.Utils;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Svg;
using Gambler.Bot.AutoBet;
using Gambler.Bot.Core.Helpers;
using ReactiveUI;
using Svg.Skia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static IronPython.Modules._ast;

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
            Gambler.Bot.AutoBet.AutoBet botIns = new Gambler.Bot.AutoBet.AutoBet(_logger);
            botIns.CompileSites();
            botIns.GetStrats();
            LoginCommand = ReactiveCommand.Create<object>(LogIn);
            SimulateCommand = ReactiveCommand.Create<object>(Simulate);
            ViewSiteCommand = ReactiveCommand.Create<object>(ViewSite);
            Sites = new ObservableCollection<AvaSitesList>(Gambler.Bot.AutoBet.AutoBet.Sites.Select(x=>new AvaSitesList(x) ));
            
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
            Image = $"Assets/Images/Currencies/{Currency.ToLower()}.svg";
            if (File.Exists(Image))
            {

            }
            else
            {

            }
            /*Image = new SKSvg();
            Uri image = new Uri($"avares://Gambler.Bot/Assets/Images/Currencies/{Currency.ToLower()}.svg");
            if (AssetLoader.Exists(image))
            {
                var asset = AssetLoader.Open(image);
                Image.Load(asset);

            }*/
        }
        /*    private DrawingImage? _photo;
        /// <summary>
        /// The photo loaded from the <see cref="PhotoUri"/>.
        /// </summary>
        public IImage Photo
        {
            get
            {
                if (_photo is null)
                {
                    var svg = new SKSvg().Load(Image);
                    // Bitmap is not an AvaloniaObject and doesn't support attached properties, so wrap it in a DrawingImage that does
                    _photo = new DrawingImage
                    {
                        Drawing = new ImageDrawing
                        {
                            
                            ImageSource = new SKSvg().Load(Image),
                            Rect = new Rect(0, 0, 192, 192)
                        }
                    };

                    // Prevent the photo from being adapted for dark themes
                    ImageProvider.SetCanAdapt(_photo, false);
                }

                return _photo;
            }
        }
        */
    
        public string Currency { get; set; }
        public string? Image 
        { 
            get; 
            set; 
        }
    }
    public class AvaGame
    {
        public AvaGame(string game)
        {
            Game = game;
            Image = $"Assets/Images/Games/{Game.ToLower()}.svg";
            /*Image = new SKSvg();
            Uri image = new Uri($"avares://Gambler.Bot/Assets/Images/Currencies/{Currency.ToLower()}.svg");
            if (AssetLoader.Exists(image))
            {
                var asset = AssetLoader.Open(image);
                Image.Load(asset);

            }*/

        }
        public string Game { get; set; }
        public string? Image
        {
            get;
            set;
        }
    }
}
