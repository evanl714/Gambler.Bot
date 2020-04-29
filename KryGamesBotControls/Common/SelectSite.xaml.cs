using DevExpress.Xpf.Grid;
using DoormatCore.Games;
using DoormatCore.Helpers;
using DoormatCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KryGamesBotControls.Common
{
    /// <summary>
    /// Interaction logic for SelectSite.xaml
    /// </summary>
    public partial class SelectSite : UserControl
    {
        public event EventHandler<SiteSelectedEventArgs> OnSiteSelected;
        private List<SitesList> sites;

        public List<SitesList> Sites
        {
            get { return sites; }
            set { sites = value; 
                gcSites.ItemsSource = sites;
            }
        }

        public SelectSite()
        {
            InitializeComponent();
            
        }

        private void tblView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            
        }

        private void CardView_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            SiteDetailControl.Content = e.NewItem;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            SitesList selectedsite = SiteDetailControl.Content as SitesList;
            //Site has been selected
            OnSiteSelected?.Invoke(this, new SiteSelectedEventArgs {
                SelectedSite = selectedsite
            }) ;
        }
        string LastSelectedGame = "";
        
        private void crncView_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            LastSelectedGame = (e.NewItem as CurrencyVM).Name;
        }
        string LastSelectedCurrency = "";
        private void GamesView_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {
            LastSelectedCurrency = (e.NewItem as CurrencyVM).Name;
        }
    }
    public class SiteSelectedEventArgs:EventArgs
    {
        public SitesList SelectedSite { get; set; }
        public string SelectedCurrency { get; set; }
        public DoormatCore.Games.Games SelectedGame { get; set; }
    }
}
