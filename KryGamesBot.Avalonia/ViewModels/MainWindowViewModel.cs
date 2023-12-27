using System.Collections.Generic;

namespace KryGamesBot.Avalonia.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        public List<string> strings { get; set; } = new List<string> { "1", "2", "three", "fuck", "you" };
    }
}