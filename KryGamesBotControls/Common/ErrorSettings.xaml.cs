using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static DoormatBot.Helpers.PersonalSettings;

namespace KryGamesBotControls.Common
{
    /// <summary>
    /// Interaction logic for ErrorSettings.xaml
    /// </summary>
    public partial class ErrorSettings : BaseControl
    {
        public List<ErrorSetting> Errors = new List<ErrorSetting>();
        public ListCollectionView lst;
        public List<ErrorActions> Actions = new List<ErrorActions>();
        public ErrorSettings()
        {
            InitializeComponent();
            DataContext = this;
            lst = new ListCollectionView(Errors);
            gcErSettings.ItemsSource = lst;
            //CollectionView  
            foreach (ErrorActions x in Enum.GetValues(typeof(ErrorActions)))
            {
                Actions.Add(x);
            }
            lueErrorAction.ItemsSource = Actions;
        }

        private void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            //gcErSettings.ItemsSource = Errors;
            
        }

        public void AddItem(ErrorSetting error)
        {
            var res = lst.AddNewItem(error);
            //gcErSettings.RefreshData();
        }
    }
}
