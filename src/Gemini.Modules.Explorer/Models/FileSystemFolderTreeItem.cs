using System;
using System.IO;

namespace Gemini.Modules.Explorer.Models
{
    public class DirectoryTreeItem : FolderTreeItem
    {
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
                if (FullPath != null && FullPath != value)
                    Directory.Move(FullPath, value);
                base.FullPath = value;
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

        public void Load(TreeItem item)
        {
            base.AddChild(item);
        }

        public static TreeItem LoadRecursive(DirectoryInfo rootDirectory)
        {
            var result = new DirectoryTreeItem()
            {
                Name = rootDirectory.Name,
                FullPath = rootDirectory.FullName,
                IsExpanded = false
            };

            foreach (var folder in rootDirectory.GetDirectories())
                result.Load(LoadRecursive(folder));
            foreach (var file in rootDirectory.GetFiles())
                result.Load(new FileSystemFileTreeItem() { Name = file.Name, FullPath = file.FullName });

            return result;
        }
    }
}
