using DevExpress.XtraRichEdit.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
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
    /// Interaction logic for ProgrammerModeConsole.xaml
    /// </summary>
    public partial class ProgrammerModeConsole : UserControl
    {
        System.Timers.Timer tmpTimer = new System.Timers.Timer(100);
        string QueuedPrints = "";
        object QueuedPrintsLock = new object();
        List<string> LastCommands = new List<string>();
        int CommandsIndex = 0;
        private Gambler.Bot.AutoBet.Strategies.ProgrammerMode strategy;

        public Gambler.Bot.AutoBet.Strategies.ProgrammerMode Strategy
        {
            get { return strategy; }
            set 
            { 
                if (strategy != null)
                {
                    strategy.OnPrint -= Strategy_OnPrint;
                    strategy.OnScriptError -= Strategy_OnScriptError;
                }
                strategy = value; if (strategy != null)
                {
                    strategy.OnPrint += Strategy_OnPrint;
                    strategy.OnScriptError += Strategy_OnScriptError;
                }
                
            }
        }

       
        private void Strategy_OnScriptError(object sender, Gambler.Bot.AutoBet.Strategies.PrintEventArgs e)
        {
            Print(e.Message);
        }

        private void Strategy_OnPrint(object sender, Gambler.Bot.AutoBet.Strategies.PrintEventArgs e)
        {
            Print(e.Message);
        }

        public ProgrammerModeConsole()
        {
            InitializeComponent();
            tmpTimer.Elapsed += TmpTimer_Elapsed;
            tmpTimer.Enabled = true;
        }

        private void TmpTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (QueuedPrints.Length > 0)
            {
                if (!Dispatcher.CheckAccess())
                {
                    Dispatcher.Invoke(new Action<object, System.Timers.ElapsedEventArgs>(TmpTimer_Elapsed), sender, e);
                    return;
                }
            
                string Message ="";
                lock (QueuedPrintsLock)
                {
                    Message = QueuedPrints;
                    QueuedPrints = "";
                }
                txtOutput.Text += Message;
                while (txtOutput.LineCount > 1000)
                {
                    string tmp = txtOutput.Text;
                    for (int i = 0; i< txtOutput.LineCount-980; i++)
                    {
                        tmp = tmp.Substring(tmp.IndexOf('\n')+1);
                    }
                    txtOutput.Text = tmp;
                }
                
            }
        }

        public void Print(string Message)
        {
            lock (QueuedPrintsLock)
            {
                QueuedPrints += Message + "\n";
            }
            /*
            if (!Dispatcher.CheckAccess()) // CheckAccess returns true if you're on the dispatcher thread
            {
                Dispatcher.Invoke(new Action<string>(Print), Message);
                return;
            }

            txtOutput.Text += Message+"\n";
            if (txtOutput.LineCount > 1000)
                txtOutput.Text = txtOutput.Text.Substring(20);*/
        }

        private void txtConsole_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void txtConsole_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                --CommandsIndex;
                if (CommandsIndex <= 0)
                    CommandsIndex = 0;
                if (CommandsIndex >= 0 && CommandsIndex < LastCommands.Count)
                {
                    txtConsole.EditValue = LastCommands[CommandsIndex];
                }
                    
            }
            if (e.Key == Key.Down)
            {
                CommandsIndex++;
                
                if (CommandsIndex >= 0 && CommandsIndex < LastCommands.Count)
                    txtConsole.EditValue = LastCommands[CommandsIndex];
                if (CommandsIndex >= LastCommands.Count)
                {
                    CommandsIndex = LastCommands.Count;
                    txtConsole.EditValue = "";
                }
            }

            if (e.Key == Key.Enter )
            {
                LastCommands.Add((string)txtConsole.EditValue);
                if (LastCommands.Count > 20)
                    LastCommands.RemoveAt(0);
                CommandsIndex = LastCommands.Count;
                Print((string)txtConsole.EditValue);
                Strategy.ExecuteCommand((string)txtConsole.EditValue);
                txtConsole.EditValue = "";
                e.Handled = true;
            }
        }

        private void txtOutput_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            try
            {
                if (txtOutput.EditCore != null)
                {
                    //txtOutput.Focus();
                    //txtOutput.SelectionStart = txtOutput.Text.Length;
                    (txtOutput.EditCore as TextBox).ScrollToEnd();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
