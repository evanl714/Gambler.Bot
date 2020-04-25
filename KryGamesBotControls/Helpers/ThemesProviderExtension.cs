using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace KryGamesBotControls.Helpers
{
    public class ThemesProviderExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            ListCollectionView collection = new ListCollectionView(Theme.Themes);
            collection.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
            return collection;

        }

        public static bool IsDark()
        {
            /*string themes = "";
            foreach (var x in Theme.Themes)
                themes += x.Name + "\n";
            System.IO.File.WriteAllText("themes.txt", themes);*/
            List<string> DarkThemes = new List<string>() { "Office2019DarkGray"
                ,"Office2019Black"
                ,"Office2019DarkGray;Touch"
                ,"Office2019Black;Touch"
                ,"VS2017Dark"
                ,"Office2016DarkGraySE"
                ,"Office2016BlackSE"
                ,"Office2016DarkGraySE;Touch"
                ,"Office2016BlackSE;Touch"
                ,"Office2016Black"
                ,"Office2016Black;Touch"
                ,"Office2013DarkGray"
                ,"Office2010Black"
                ,"MetropolisDark"
                ,"Office2007Black"
                ,"TouchlineDark"
                ,"DeepBlue"
            };
            return  (DarkThemes.Contains(ApplicationThemeHelper.ApplicationThemeName)) ;
        }
    }

   
}
