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
    /// Interaction logic for ProgrammerModeConsole.xaml
    /// </summary>
    public partial class ProgrammerModeConsole : UserControl
    {

        List<string> LastCommands = new List<string>();
        int CommandsIndex = 0;
        private DoormatBot.Strategies.ProgrammerMode strategy;

        public DoormatBot.Strategies.ProgrammerMode Strategy
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

       
        private void Strategy_OnScriptError(object sender, DoormatBot.Strategies.PrintEventArgs e)
        {
            Print(e.Message);
        }

        private void Strategy_OnPrint(object sender, DoormatBot.Strategies.PrintEventArgs e)
        {
            Print(e.Message);
        }

        public ProgrammerModeConsole()
        {
            InitializeComponent();
        }

        public void Print(string Message)
        {
            txtOutput.Text += Message+"\n";
            if (txtOutput.LineCount > 1000)
                txtOutput.Text = txtOutput.Text.Substring(20);
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
    }
}
