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
        private string _oldFullPath;
        public FileSystemFileTreeItem(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }
        public override bool IsEditing
        {
            get => base.IsEditing;
            set
            {
                base.IsEditing = value;
                if (IsEditing)
                {
                    _oldFullPath = FullPath;
                }
                else
                {
                    var directoryName = Path.GetDirectoryName(FullPath);
                    FullPath = Path.Combine(directoryName, Name);
                    File.Move(_oldFullPath, FullPath);
                    _oldFullPath = null;
                }
            }
        }
        public override Uri IconSource => GetIconSource();
        public override bool CanOpenDocument => true;

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
