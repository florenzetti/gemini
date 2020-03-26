using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    public sealed class FolderTemplate : EditorFileTemplate
    {
        public new string Name => "Folder";
        public override Uri IconSource16 => new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/folder.png");
        public override Uri IconSource32 => new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/folder.png");

        private FolderTemplate()
        { }

        [Export]
        public static EditorFileTemplate Template = new FolderTemplate();
    }

    public sealed class DefaultFileTemplate : EditorFileTemplate
    {
        public new string Name => "DefaultFile";
        public override Uri IconSource16 => new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/document-16.png");
        public override Uri IconSource32 => new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/document-32.png");

        private DefaultFileTemplate()
        { }

        [Export]
        public static EditorFileTemplate Template = new DefaultFileTemplate();
    }
}
