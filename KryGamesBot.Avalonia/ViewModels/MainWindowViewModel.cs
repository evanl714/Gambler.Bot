using System.Collections.Generic;

namespace KryGamesBot.Ava.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        public List<string> strings { get; set; } = new List<string> { "1", "2", "three", "fuck", "you" };
    }
}