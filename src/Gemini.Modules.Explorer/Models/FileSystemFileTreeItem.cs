using Caliburn.Micro;
using Gemini.Framework.Commands;
using Gemini.Modules.Explorer.Commands;
using Gemini.Modules.MainMenu.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gemini.Modules.Explorer.Models
{
    public class FileSystemFileTreeItem : TreeItem
    {
        public override Uri IconSource => GetIconSource();
        public override bool CanOpenDocument => true;
        public override string Name
        {
            get => base.Name;
            set
            {
                if (Name != null && Name != value)
                {
                    var directoryPath = Path.GetDirectoryName(FullPath);
                    FullPath = Path.Combine(directoryPath, value);
                }
                base.Name = value;
            }
        }
        public override string FullPath
        {
            get => base.FullPath;
            set
            {
                if(FullPath != null && FullPath != value)
                    File.Move(FullPath, value);
                base.FullPath = value;
            }
        }

        public override void RemoveChild(TreeItem item)
        {
            if (File.Exists(item.FullPath))
                File.Delete(item.FullPath);

            base.RemoveChild(item);
        }

        protected virtual Uri GetIconSource()
        {
            //TODO: add more icons
            var icon = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/file.png");
            return icon;
        }
    }
}
