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
    public class FileSystemTreeItem : TreeItem
    {
        //public Guid DocumentId { get; set; }
        private string _name;
        public override string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        private string _fullPath;
        public override string FullPath
        {
            get => _fullPath;
            set
            {
                _fullPath = value;
                NotifyOfPropertyChange(() => FullPath);
            }
        }
        public override Uri IconSource => GetIconSource();
        public override bool CanOpenDocument => !IsFolder;
        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                NotifyOfPropertyChange(() => IsExpanded);
                NotifyOfPropertyChange(() => IconSource);
            }
        }
        private readonly BindableCollection<TreeItem> _children;
        public override IList<TreeItem> Children => _children;

        //public override IEnumerable<CommandMenuItem> Commands
        //{
        //    get
        //    {
        //        yield return new CommandMenuItem();
        //    }
        //}

        public bool IsFolder { get; private set; }

        public FileSystemTreeItem()
        {
            _children = new BindableCollection<TreeItem>();
        }

        public override TreeItem FindChildRecursive(string fullPath)
        {
            return FindChildRecursive(fullPath, this);
        }

        public override TreeItem AddChild(string fullPath)
        {
            var parentPath = Path.GetDirectoryName(fullPath);
            var parentTreeItem = FindChildRecursive(parentPath);
            if (parentTreeItem == null)
                return null;

            var attributes = File.GetAttributes(fullPath);
            var result = new FileSystemTreeItem()
            {
                Name = Path.GetFileName(fullPath),
                FullPath = fullPath,
                IsFolder = attributes.HasFlag(FileAttributes.Directory)
            };
            parentTreeItem.Children.Add(result);

            return result;
        }

        public override void RemoveChild(string fullPath)
        {
            var parentFolderName = Path.GetDirectoryName(fullPath);
            var parentTreeItem = FindChildRecursive(parentFolderName);
            if (parentTreeItem != null)
            {
                var treeItem = parentTreeItem.Children.SingleOrDefault(o => o.FullPath == fullPath);
                parentTreeItem.Children.Remove(treeItem);
            }
        }

        public static FileSystemTreeItem LoadRecursive(DirectoryInfo rootDirectory)
        {
            var result = new FileSystemTreeItem()
            {
                Name = rootDirectory.Name,
                FullPath = rootDirectory.FullName,
                IsExpanded = false,
                IsFolder = true
            };

            foreach (var folder in rootDirectory.GetDirectories())
                result.Children.Add(LoadRecursive(folder));
            foreach (var file in rootDirectory.GetFiles())
                result.Children.Add(new FileSystemTreeItem() { Name = file.Name, FullPath = file.FullName, IsFolder = false });

            return result;
        }

        protected static TreeItem FindChildRecursive(string fullPath, TreeItem root)
        {
            TreeItem result = null;
            if (root.FullPath == fullPath)
                result = root;
            else
            {
                result = root.Children.SingleOrDefault(o => o.FullPath == fullPath);
                if (result == null)
                {
                    foreach (var folder in root.Children.Where(o => o.Children.Any()).ToList())
                    {
                        FindChildRecursive(fullPath, folder);
                    }
                }
            }
            return result;
        }

        protected virtual Uri GetIconSource()
        {
            var icon = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/file.png");
            if (IsFolder)
            {
                if (IsExpanded)
                    icon = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/folder_open.png");
                else
                    icon = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/folder.png");
            }
            return icon;
        }
    }
}
