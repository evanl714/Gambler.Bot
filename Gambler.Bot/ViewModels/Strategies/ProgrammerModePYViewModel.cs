using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gambler.Bot.ViewModels.Strategies
{
    public class ProgrammerModePYViewModel:ProgrammerModeViewModel
    {
        public ProgrammerModePYViewModel(Microsoft.Extensions.Logging.ILogger logger) : base(logger)
        {
            
        }
        protected override void SetLanguage()
        {

            LanguageName = "Python";
            FileExtension = "py";
            TemplateName = "PYTemplate.py";
        }    
    }
}
