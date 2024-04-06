using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.Strategies
{
    public class ProgrammerModeCSViewModel:ProgrammerModeViewModel
    {
        protected override void SetLanguage()
        {
            
            LanguageName = "C#";
            FileExtension = "cs";            
            TemplateName = "CSTemplate.cs";
        }    
    }
}
