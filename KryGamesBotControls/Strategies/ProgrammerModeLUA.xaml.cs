using DevExpress.Xpf.RichEdit;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Commands;
using DevExpress.XtraRichEdit.Services;
using DoormatBot.Strategies;
using KryGamesBotControls.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
        protected string FileExtension { get; set; } = "lua";
        protected string LanguageName { get; set; } = "LUA";
        protected string TemplateName { get; set; } = "LUATemplate.lua";


        DoormatBot.Strategies.ProgrammerMode Strat = null;
        DateTime LastChanged = DateTime.Now;
        FileSystemWatcher FileWatcher;
        public ProgrammerModeLUA()
        {
            InitializeComponent();
            SetLanguage();
            richEditControl1.DocumentSaveOptions.DefaultFormat = DocumentFormat.PlainText;
            richEditControl1.ApplyTemplate();
            CustomRichEditCommandFactoryService commandFactory =
                  new CustomRichEditCommandFactoryService(richEditControl1,
                      richEditControl1.GetService<IRichEditCommandFactoryService>(), FileExtension, LanguageName);
            richEditControl1.ReplaceService<IRichEditCommandFactoryService>(commandFactory);
            richEditControl1.Document.DefaultTabWidth = 5;
            richEditControl1.HorizontalRulerOptions.Visibility = RichEditRulerVisibility.Hidden;
            richEditControl1.VerticalRulerOptions.Visibility = RichEditRulerVisibility.Hidden;
            richEditControl1.Height = this.Height;
            richEditControl1.Width = this.Width;
            this.SizeChanged += ProgrammerModeLUA_SizeChanged;
            CreateWatcher();
        }

        protected virtual void SetLanguage()
        {
            FileExtension = "lua";
            LanguageName = "LUA";
            TemplateName = "LUATemplate.lua";
        }

        void CreateWatcher()
        {
            if (FileWatcher != null)
            {
                FileWatcher.Dispose();
            }
            if (Strat!=null && File.Exists(Strat.FileName))
            {
                FileWatcher = new FileSystemWatcher(System.IO.Path.GetDirectoryName(Strat.FileName), System.IO.Path.GetFileName(Strat.FileName));
                FileWatcher.Changed += FileWatcher_Changed;
                FileWatcher.EnableRaisingEvents = true;
            }
        }

        private void FileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
                LoadDocument();
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
            Strat = Strategy as ProgrammerMode;
            LoadDocument();
            CreateWatcher();
            //throw new NotImplementedException();
        }
        void LoadDocument()
        {
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(LoadDocument);
                return;
            }
            if (Strat?.FileName != null)
            {
                string path = Strat?.FileName;
                if (Strat != null)
                    Strat.FileName = path;
                try
                {
                    richEditControl1.LoadDocument(path, DocumentFormat.PlainText);
                    richEditControl1.DocumentSaveOptions.CurrentFormat = DocumentFormat.PlainText;
                    richEditControl1.DocumentSaveOptions.DefaultFormat = DocumentFormat.PlainText;
                    txtFileName.Text = System.IO.Path.GetFileName(richEditControl1.DocumentSaveOptions.CurrentFileName);
                    richEditControl1.ReadOnly = false;
                }
                catch
                {

                }
            }
        }

        

        private void richEditControl1_Loaded(object sender, RoutedEventArgs e)
        {
            richEditControl1.ActiveView.AdjustColorsToSkins = true;
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
            richEditControl1.Document.Sections[0].LineNumbering.RestartType = DevExpress.XtraRichEdit.API.Native.LineNumberingRestart.Continuous;
            richEditControl1.Document.Sections[0].LineNumbering.Start = 1;
            richEditControl1.Document.Sections[0].LineNumbering.CountBy = 1;
            richEditControl1.Document.Sections[0].LineNumbering.Distance = 25;
            richEditControl1.Document.CharacterStyles["Line Number"].Bold = true;
            richEditControl1.Document.CharacterStyles["Line Number"].ForeColor = System.Drawing.Color.DarkGray ;
        }

        private void richEditControl1_TextChanged(object sender, EventArgs e)
        {
            DateTime LastChanged = DateTime.Now;
        }
        public bool TopAlign()
        {
            return false;
        }

        public void Saving()
        {
            WriteDocument();
        }

        void WriteDocument()
        {
            btnSave_Click(btnSave, new RoutedEventArgs());
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dg = new SaveFileDialog();
            dg.Filter = $"{LanguageName} Code File (*.{FileExtension})|*.{FileExtension}";
            dg.AddExtension = true;
            if (dg.ShowDialog() ?? false)
            {
                string FileName = dg.FileName;
                Stream x = typeof(BaseStrategy).Assembly.GetManifestResourceStream($"Doormat.Bot.Samples.{TemplateName}");
                string[] items = typeof(BaseStrategy).Assembly.GetManifestResourceNames();
                byte[] buffer = new byte[x.Length];
                x.Read(buffer, 0,(int) x.Length);
                File.WriteAllBytes(FileName,buffer);
                Strat.FileName = FileName;
                LoadDocument();
                CreateWatcher();
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {   
            richEditControl1.CreateCommand( RichEditCommandId.FileOpen ).Execute();
            //richEditControl1.LoadDocument();
            string FileName = richEditControl1.DocumentSaveOptions.CurrentFileName;
            Strat.FileName = FileName;
            CreateWatcher();            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            richEditControl1.CreateCommand(RichEditCommandId.FileSave).Execute();
        }

        private void btnSaveAs_Click(object sender, RoutedEventArgs e)
        {
            richEditControl1.CreateCommand(RichEditCommandId.FileSaveAs).Execute();

            string FileName = richEditControl1.DocumentSaveOptions.CurrentFileName;            
            Strat.FileName = FileName;
            CreateWatcher();            
        }
    }
    public class CustomRichEditCommandFactoryService : IRichEditCommandFactoryService
    {
        public string Language { get; set; }
        public string Extension { get; set; }
        readonly IRichEditCommandFactoryService service;
        readonly RichEditControl control;
        public CustomRichEditCommandFactoryService(RichEditControl control, IRichEditCommandFactoryService service, string fileExtension, string languageName)
        {
            DevExpress.Utils.Guard.ArgumentNotNull(control, "control");
            DevExpress.Utils.Guard.ArgumentNotNull(service, "service");
            this.control = control;
            this.service = service;
            this.Language = languageName;
            this.Extension = fileExtension;
        }
        public RichEditCommand CreateCommand(RichEditCommandId id)
        {
            if (id == RichEditCommandId.FileSaveAs)
                return new CustomSaveDocumentAsCommand(control, Language, Extension);
            if (id == RichEditCommandId.FileOpen)
                return new CustomOpendocumentcommand(control, Language, Extension);
            if (id == RichEditCommandId.FileSave)
                return new CustomSaveDocumentcommand(control, Language, Extension);
            return service.CreateCommand(id);
        }
    }
    public class CustomOpendocumentcommand:DevExpress.XtraRichEdit.Commands.LoadDocumentCommand
    {
        public string Language { get; set; }
        public string Extension { get; set; }
        public CustomOpendocumentcommand(IRichEditControl control, string Language, string Extension)
            : base(control)
        {
            this.Language = Language;
            this.Extension = Extension;
        }

        protected override void ExecuteCore()
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Filter = $"{Language} Code Files (*.{Extension})|*.{Extension}",
                //FileName = "SavedDocument.rtf",
                RestoreDirectory = true,
                CheckFileExists = false,
                CheckPathExists = true,
                DereferenceLinks = true,
                ValidateNames = true,
                AddExtension = false,
                FilterIndex = 1
            };
            //dialog.InitialDirectory = "C:\\Temp";
            if (dialog.ShowDialog() ?? false)
            {
                ((RichEditControl)this.Control).LoadDocument(dialog.FileName, DocumentFormat.PlainText);
            }
        }
    }
    public class CustomSaveDocumentcommand : DevExpress.XtraRichEdit.Commands.SaveDocumentCommand
    {
       
        public CustomSaveDocumentcommand(IRichEditControl control, string Language, string Extension)
            : base(control)
        {
            
        }

        protected override void ExecuteCore()
        {

            ((RichEditControl)this.Control).DocumentSaveOptions.CurrentFormat = DocumentFormat.PlainText;
            ((RichEditControl)this.Control).DocumentSaveOptions.DefaultFormat = DocumentFormat.PlainText;
            ((RichEditControl)this.Control).SaveDocument();
            
        }
    }
    public class CustomSaveDocumentAsCommand : SaveDocumentAsCommand
    {
        public string Language { get; set; }
        public string Extension { get; set; }
        public CustomSaveDocumentAsCommand(IRichEditControl control,string Language,string Extension)
            : base(control) {
            this.Language = Language;
            this.Extension = Extension;
        }

        protected override void ExecuteCore()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                Filter = $"{Language} Code Files (*.{Extension})|*.{Extension}",
                //FileName = "SavedDocument.rtf",
                RestoreDirectory = true,
                CheckFileExists = false,
                CheckPathExists = true,
                OverwritePrompt = true,
                DereferenceLinks = true,
                ValidateNames = true,
                AddExtension = false,
                FilterIndex = 1
            };
            //dialog.InitialDirectory = "C:\\Temp";
            if (dialog.ShowDialog()??false)
            {
                ((RichEditControl)this.Control).SaveDocument(dialog.FileName, DocumentFormat.PlainText);
            }
            //base.ExecuteCore(); 
        }
    }
}


