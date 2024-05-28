using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Gambler.Bot.Core.Helpers;
using Org.BouncyCastle.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextMateSharp.Model;

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
