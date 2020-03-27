using Gemini.Framework.Services;
using System;

namespace Gemini.Modules.Explorer.Models
{
    public class EditorFileTemplate : EditorFileType
    {
        public string Description { get; set; }
        public string InitContent { get; set; }
        public virtual Uri IconSource32 { get; set; }

        public EditorFileTemplate()
        { }
        public EditorFileTemplate(string name, string fileExtension, string description) : base(name, fileExtension)
        {
            Description = description;
        }

        public override bool Equals(object obj)
        {
            return obj is EditorFileTemplate && ((EditorFileTemplate)obj).Name == Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

    public sealed class DefaultFileTemplate : EditorFileTemplate
    {
        private DefaultFileTemplate()
        {
            Name = "Default file";
            IconSource32 = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/document-32.png");
        }

        public static EditorFileTemplate DefaultTemplate = new DefaultFileTemplate();
    }

    public sealed class DefaultFolderTemplate : EditorFileTemplate
    {
        private DefaultFolderTemplate()
        {
            Name = "Folder";
            IconSource32 = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/folder.png");
        }

        public static EditorFileTemplate DefaultTemplate = new DefaultFolderTemplate();
    }    
}
