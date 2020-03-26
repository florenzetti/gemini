using System;
using System.IO;

namespace Gemini.Modules.Explorer.Models
{
    public class FileSystemFolderTreeItem : FolderTreeItem
    {
        private string _oldFullPath;

        public override bool IsRootFolder => false;

        internal FileSystemFolderTreeItem(string name, string fullPath)
        {
            Name = name;
            FullPath = fullPath;
        }
        //public override bool IsEditing
        //{
        //    get => base.IsEditing;
        //    set
        //    {
        //        base.IsEditing = value;
        //        if (IsEditing)
        //        {
        //            _oldFullPath = FullPath;
        //        }
        //        else
        //        {
        //            var directoryName = Path.GetDirectoryName(FullPath);
        //            var newFullPath = Path.Combine(directoryName, Name);
        //            if (_oldFullPath != newFullPath)
        //            {
        //                FullPath = newFullPath;
        //                Directory.Move(_oldFullPath, FullPath);
        //            }
        //            _oldFullPath = null;
        //        }
        //    }
        //}

        //public override void MoveTo(TreeItem moveToItem)
        //{
        //    if (Parent == moveToItem || FindChildRecursive(moveToItem.FullPath) != null)
        //        return;

        //    var newFullPath = Path.Combine(moveToItem.FullPath, Name);
        //    Directory.Move(FullPath, newFullPath);
        //    FullPath = newFullPath;
        //    base.MoveTo(moveToItem);
        //}

        //public override void RemoveChild(TreeItem item)
        //{
        //    if (Directory.Exists(item.FullPath))
        //        Directory.Delete(item.FullPath, true);
        //    else if (File.Exists(item.FullPath))
        //        File.Delete(item.FullPath);

        //    base.RemoveChild(item);
        //}

        //public override void AddChild(TreeItem item)
        //{
        //    if (Directory.Exists(Path.GetDirectoryName(item.FullPath)))
        //    {
        //        File.Create(item.FullPath);
        //    }
        //    base.AddChild(item);
        //}

        public static FolderTreeItem LoadRecursive(DirectoryInfo rootDirectory)
        {
            return LoadRecursive(rootDirectory, false);
        }

        public static FolderTreeItem LoadRecursive(DirectoryInfo rootDirectory, bool isRootExpanded)
        {
            var result = new FileSystemFolderTreeItem(rootDirectory.Name, rootDirectory.FullName)
            {
                IsExpanded = isRootExpanded
            };

            foreach (var folder in rootDirectory.GetDirectories())
                result.LoadChild(LoadRecursive(folder, false));
            foreach (var file in rootDirectory.GetFiles())
                result.LoadChild(new FileSystemFileTreeItem(file.Name, file.FullName));

            return result;
        }
    }
}
