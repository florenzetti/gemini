using System;
using System.IO;

namespace Gemini.Modules.Explorer.Models
{
    public class DirectoryTreeItem : FolderTreeItem
    {
        private string _oldFullPath;

        public DirectoryTreeItem(string name, string fullPath)
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
                    Directory.Move(_oldFullPath, FullPath);
                    _oldFullPath = null;
                }
            }
        }

        public override void RemoveChild(TreeItem item)
        {
            if (Directory.Exists(item.FullPath))
                Directory.Delete(item.FullPath, true);
            base.RemoveChild(item);
        }

        public override void AddChild(TreeItem item)
        {
            if (Directory.Exists(Path.GetDirectoryName(item.FullPath)))
            {
                File.Create(item.FullPath);
            }
            base.AddChild(item);
        }

        //public void Load(TreeItem item)
        //{
        //    base.AddChild(item);
        //}

        public static TreeItem LoadRecursive(DirectoryInfo rootDirectory)
        {
            var result = new DirectoryTreeItem(rootDirectory.Name, rootDirectory.FullName)
            {
                IsExpanded = false
            };

            foreach (var folder in rootDirectory.GetDirectories())
                result.LoadChild(LoadRecursive(folder));
            foreach (var file in rootDirectory.GetFiles())
                result.LoadChild(new FileSystemFileTreeItem(file.Name, file.FullName));

            return result;
        }
    }
}
