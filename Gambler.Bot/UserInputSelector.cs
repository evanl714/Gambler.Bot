using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Gambler.Bot.ViewModels;
using Gambler.Bot.ViewModels.AppSettings;
using Gambler.Bot.ViewModels.Common;
using Gambler.Bot.Views.AppSettings;
using System;
using System.Collections.Generic;

namespace Gambler.Bot
{
    public class UserInputSelector : IDataTemplate
    {
        public bool SupportsRecycling => false;
        [Content]
        public Dictionary<int, IDataTemplate> Templates { get; } = new Dictionary<int, IDataTemplate>();

        public Control Build(object data)
        {
            return Templates[((UserInputViewModel)data).Args.DataType].Build(data);
        }

        public bool Match(object data)
        {
            return data is UserInputViewModel;
        }
    }
}