using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Framework
{
    public static class IconSourceUtility
    {
        public static Uri GetByExtension(string extension, int dimension)
        {
            //TODO: add more icons
            return new Uri($"pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/document-{dimension}.png");
        }
    }
}
