using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DevExpress.XtraRichEdit.Services;
using DoormatBot.Strategies;
using KryGamesBotControls.Helpers;
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

namespace KryGamesBotControls.Strategies
{
    /// <summary>
    /// Interaction logic for ProgrammerModeLUA.xaml
    /// </summary>
    public partial class ProgrammerModeLUA : UserControl, iStrategy
    {
        DoormatBot.Strategies.ProgrammerLUA Strat = null;
        DateTime LastChanged = DateTime.Now;
        public ProgrammerModeLUA()
        {
            InitializeComponent();
            richEditControl1.HorizontalRulerOptions.Visibility = RichEditRulerVisibility.Hidden;
            richEditControl1.VerticalRulerOptions.Visibility = RichEditRulerVisibility.Hidden;
            richEditControl1.Height = this.Height;
            richEditControl1.Width = this.Width;
            this.SizeChanged += ProgrammerModeLUA_SizeChanged;
            
        }

        private void ProgrammerModeLUA_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            richEditControl1.Height = e.NewSize.Height;
            richEditControl1.Width = e.NewSize.Width;
        }

        public event EventHandler StartBetting;

        public void GameChanged(DoormatCore.Games.Games newGame)
        {
            //throw new NotImplementedException();
        }

        public void SetStrategy(BaseStrategy Strategy)
        {
            Strat = Strategy as ProgrammerLUA;
            LoadDocument();
            //throw new NotImplementedException();
        }
        void LoadDocument()
        {
            if (Strat?.FileName != null)
            {
                string path = Strat?.FileName;
                if (Strat != null)
                    Strat.FileName = path;

                richEditControl1.LoadDocument(path, DocumentFormat.PlainText);
            }
        }

        private void richEditControl1_Loaded(object sender, RoutedEventArgs e)
        {
            if (richEditControl1.ActiveView != null)
            {
                richEditControl1.ReplaceService<ISyntaxHighlightService>(new MySyntaxHighlightService(richEditControl1));
                richEditControl1.Views.SimpleView.AllowDisplayLineNumbers = true;
                richEditControl1.Views.DraftView.AllowDisplayLineNumbers = true;
                richEditControl1.Views.PrintLayoutView.AllowDisplayLineNumbers = true;
                richEditControl1.Views.SimpleView.Padding = new System.Windows.Forms.Padding(40, 4, 0, 0);
            }
            LoadDocument();
        }

        private void richEditControl1_DocumentLoaded(object sender, EventArgs e)
        {
            richEditControl1.Document.Sections[0].LineNumbering.RestartType = LineNumberingRestart.Continuous;
            richEditControl1.Document.Sections[0].LineNumbering.Start = 1;
            richEditControl1.Document.Sections[0].LineNumbering.CountBy = 1;
            richEditControl1.Document.Sections[0].LineNumbering.Distance = 0.1f;
            
        }

        private void richEditControl1_TextChanged(object sender, EventArgs e)
        {
            DateTime LastChanged = DateTime.Now;
        }
    }
}


