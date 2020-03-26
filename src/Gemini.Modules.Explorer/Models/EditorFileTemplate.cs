using Gemini.Framework.Services;
using System;

namespace Gemini.Modules.Explorer.Models
{
    public class EditorFileTemplate : EditorFileType
    {
        public string Description { get; set; }
        public string FileContent { get; set; }
        public virtual Uri IconSource16
        {
            get { return new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/document-16.png"); }
        }
        public virtual Uri IconSource32
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
