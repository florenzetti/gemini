using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public static class FileTemplates
    {
        [Export]
        public static EditorFileTemplate FolderTemplate = new EditorFileTemplate()
        {
            Name = "Folder"
        };


    }
}
