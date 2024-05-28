using Avalonia.Controls;
using Avalonia.Controls.Templates;
using KryGamesBot.Ava.ViewModels;
using KryGamesBot.Ava.ViewModels.AppSettings;
using KryGamesBot.Ava.Views.AppSettings;
using System;

namespace KryGamesBot.Ava
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                if (data is SQLServerViewModel)
                {
                    return new SQLServerView();                        
                }
            }

            return new TextBlock { Text = "Not Found: " + name };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}