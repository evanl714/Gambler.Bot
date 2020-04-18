using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBotControls.Strategies
{
    public class ProgrammerModeCS:ProgrammerModeLUA
    {
        protected override void SetLanguage()
        {
            LanguageName = "C#";
            FileExtension = "cs";
            TemplateName = "CSTemplate.cs";
        }
    }
}
