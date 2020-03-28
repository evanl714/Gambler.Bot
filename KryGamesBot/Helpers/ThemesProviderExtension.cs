using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace KryGamesBot.Helpers
{
    public class ThemesProviderExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            ListCollectionView collection = new ListCollectionView(Theme.Themes);
            collection.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            return collection;

        }
    }
}
