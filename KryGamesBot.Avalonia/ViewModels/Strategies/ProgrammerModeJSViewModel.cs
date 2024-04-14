using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KryGamesBot.Ava.ViewModels.Strategies
{
    public class ProgrammerModeJSViewModel:ProgrammerModeViewModel
    {
        public ProgrammerModeJSViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            
        }
        protected override void SetLanguage()
        {

            LanguageName = "Javascript";
            FileExtension = "js";
            TemplateName = "JSTemplate.js";
        }    
    }
}
