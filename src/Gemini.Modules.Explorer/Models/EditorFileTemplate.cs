using Gemini.Framework.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    public class EditorFileTemplate : EditorFileType
    {
        public string Description { get; set; }
        public string Template { get; set; }
        public virtual Uri IconSource
        {
            get { return new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/document-32.png"); }
        }

        public EditorFileTemplate() { }
        public EditorFileTemplate(string name, string fileExtension, string description) : base(name, fileExtension)
        {
            Description = description;
        }
    }
}
