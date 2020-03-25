using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.CodeEditor
{
    public class ExcludeLanguageDefinition
    {
        public string LanguageName { get; }

        public ExcludeLanguageDefinition(string languageName)
        {
            LanguageName = languageName;
        }
    }
}
