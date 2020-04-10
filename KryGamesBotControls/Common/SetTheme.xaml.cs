using DevExpress.Xpf.Core;
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
    /// Interaction logic for SetTheme.xaml
    /// </summary>
    public partial class SetTheme : UserControl
    {
        public SetTheme()
        {
            InitializeComponent();
            tbTheme.Text = "You can select from a list of themes and skins for KryGamesBot.";
        }
        private void cbeTheme_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            ApplicationThemeHelper.ApplicationThemeName = cbeTheme.EditValue?.ToString();
        }
    }
}
