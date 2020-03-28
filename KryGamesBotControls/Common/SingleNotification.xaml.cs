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
    /// Interaction logic for SingleNotification.xaml
    /// </summary>
    public partial class SingleNotification : UserControl
    {
        public Trigger NotificationTrigger { get; set; }
        public SingleNotification()
        {
            InitializeComponent();
        }
    }
}
