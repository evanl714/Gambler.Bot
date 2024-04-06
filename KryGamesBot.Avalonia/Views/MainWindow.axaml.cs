using Avalonia.Controls;

namespace KryGamesBot.Ava.Views
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