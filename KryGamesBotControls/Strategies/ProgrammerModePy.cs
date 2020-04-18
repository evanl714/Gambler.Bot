using System;
using System.Collections.Generic;
using System.Text;

namespace KryGamesBotControls.Strategies
{
    public class ProgrammerModePy : ProgrammerModeLUA
    {
        protected override void SetLanguage()
        {
            LanguageName = "Python";
            FileExtension = "py";
            TemplateName = "PYTemplate.py";
        }
    }   
}
