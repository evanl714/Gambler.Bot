using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.Strategies
{
    public class ProgrammerModeCSViewModel:ProgrammerModeViewModel
    {
        public ProgrammerModeCSViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            
        }
        protected override void SetLanguage()
        {
            
            LanguageName = "C#";
            FileExtension = "cs";            
            TemplateName = "CSTemplate.cs";
        }    
    }
}
