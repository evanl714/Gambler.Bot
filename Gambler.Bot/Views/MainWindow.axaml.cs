using Avalonia.Controls;

namespace Gambler.Bot.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object? sender, WindowClosingEventArgs e)
        {
            
        }
    }
}