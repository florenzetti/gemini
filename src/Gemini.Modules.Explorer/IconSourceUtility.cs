using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer
{
    public static class IconSourceUtility
    {
        public static Uri GetByExtension(string extension)
        {
            return new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/document-32.png");
        }
    }
}
