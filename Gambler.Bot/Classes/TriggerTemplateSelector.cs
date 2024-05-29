using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Gambler.Bot.Core.Helpers;
using System.Collections.Generic;

namespace Gambler.Bot.Classes
{
    public class BetTriggerTemplateSelector : IDataTemplate
    {
        public bool SupportsRecycling => false;
        [Content]
        public Dictionary<CompareAgainst, IDataTemplate> Templates { get; } = new Dictionary<CompareAgainst, IDataTemplate>();

        public bool Match(object data)
        {
            return data is CompareAgainst;
        }

        Control? ITemplate<object?, Control?>.Build(object? param)
        {
            return Templates[((CompareAgainst)param)].Build(param);
        }
    }
}
