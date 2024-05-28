using DevExpress.Xpf.Core;
using DevExpress.XtraCharts.Native;
using Gambler.Bot.AutoBet;
using Gambler.Bot.AutoBet.Helpers;
using KryGamesBotControls.Common;
using Microsoft.Scripting.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KryGamesBot
{
    /// <summary>
    /// Interaction logic for GlobalSettings.xaml
    /// </summary>
    public partial class GlobalSettings : DXWindow, INotifyPropertyChanged
    {
        public PersonalSettings Settings { get; set; }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    
        public SettingNode SelectedNode { get { return TreeListView.DataControl.SelectedItem as SettingNode; } }
        public List<SettingNode> nodes { get; set; } = new List<SettingNode>() { };
        public string[] SettingItems { get; set; }
        public GlobalSettings()
        {
            
            nodes.Add(new SettingNode { Id = 1, Name = "Skin", UserControl= new SetTheme() });
            nodes.Add(new SettingNode { Id = 2, Name = "KeePass", UserControl= new KeePassSettings() { FileName="" } });            
            nodes.Add(new SettingNode { Id = 3, Name = "Storage" });
            nodes.Add(new SettingNode { Id = 4, Name = "Bets", ParentId = 3, UserControl= new DatabaseSetup() });
            nodes.Add(new SettingNode { Id = 5, Name = "Strategies", ParentId = 3 });
            nodes.Add(new SettingNode { Id = 6, Name = "Notifications" });
            nodes.Add(new SettingNode { Id = 7, Name = "Updates" });
            nodes.Add(new SettingNode { Id = 8, Name = "Live Bets", UserControl= new LiveBetSettings() });            
            nodes.Add(new SettingNode { Id = 11, Name = "Donate" });
            nodes.Add(new SettingNode { Id = 12, Name = "Proxy" });            
            var ErrSetts = new ErrorSettings();
            
            nodes.Add(new SettingNode { Id = 13, Name = "Errors", UserControl = ErrSetts });

            InitializeComponent();
            this.Loaded += GlobalSettings_Loaded;
            //SettingItems =  new string[]{"Skin","KeePass","Bet Storage","Errors","Notifications","Updates","Live View","Donate","Proxy","Strategy Storage" };
            DataContext = this;
        }

        private void GlobalSettings_Loaded(object sender, RoutedEventArgs e)
        {
            SettingNode tmpNode = nodes.First(m => m.Name == "Errors");
            if (tmpNode.UserControl is ErrorSettings ErrSetts && Settings!=null)
            foreach (var x in Settings.ErrorSettings)
            {
                ErrSetts.AddItem(x);
            }
        }

        private void TreeListControl_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
         {
            OnPropertyChanged("SelectedNode");
        }
    }

    public class SettingNode
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public Control UserControl { get; set; }


    }
}

/*<dxg:TreeListView.Nodes>
                                    <dxg:TreeListNode Content="Skin" >
                                       
                                    </dxg:TreeListNode>
                                    <dxg:TreeListNode Content="KeePass">

                                    </dxg:TreeListNode>
                                    <dxg:TreeListNode Content="Storage">
                                        <dxg:TreeListNode.Nodes>
                                            <dxg:TreeListNode Content="Bets">
                                            </dxg:TreeListNode>
                                            <dxg:TreeListNode Content="Strategies">
                                            </dxg:TreeListNode>
                                        </dxg:TreeListNode.Nodes>
                                    </dxg:TreeListNode>
                                    <dxg:TreeListNode Content="Notifications">
                                    </dxg:TreeListNode>
                                    <dxg:TreeListNode Content="Updates">
                                    </dxg:TreeListNode>
                                    <dxg:TreeListNode Content="Live Bets">
                                        <dxg:TreeListNode.Nodes>
                                            <dxg:TreeListNode Content="Chart">
                                            </dxg:TreeListNode>
                                            <dxg:TreeListNode Content="Feed">
                                            </dxg:TreeListNode>
                                        </dxg:TreeListNode.Nodes>
                                    </dxg:TreeListNode>
                                    <dxg:TreeListNode Content="Donate">
                                    </dxg:TreeListNode>
                                    <dxg:TreeListNode Content="Proxy">
                                    </dxg:TreeListNode>
                                </dxg:TreeListView.Nodes>
                            </dxg:TreeListView>
*/