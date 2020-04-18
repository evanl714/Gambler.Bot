using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBotControls.Strategies
{
    public class ProgrammerModeJS:ProgrammerModeLUA
    {
        protected override void SetLanguage()
        {
            LanguageName = "Javascript";
            FileExtension = "js";
            TemplateName = "JSTemplate.js";
        }
    }
}
