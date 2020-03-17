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

        public bool IsFolder { get; private set; }

        public override void AddChild(TreeItem item)
        {
            //var parentPath = Path.GetDirectoryName(item.FullPath);
            //var parentTreeItem = FindChildRecursive(parentPath);
            //if (parentTreeItem == null)
            //    return null;
            //File.Create(item.FullPath);
            //var attributes = File.GetAttributes(item.FullPath);
            var result = new FileSystemTreeItem()
            {
                Name = Path.GetFileName(item.FullPath),
                FullPath = item.FullPath//,
                //IsFolder = attributes.HasFlag(FileAttributes.Directory)
            };
            base.AddChild(result);
        }

        public override void RemoveChild(TreeItem item)
        {
            if (File.Exists(item.FullPath))
                File.Delete(item.FullPath);
            else if (Directory.Exists(item.FullPath))
                Directory.Delete(item.FullPath, true);

            base.RemoveChild(item);
        }

        public void Load(FileSystemTreeItem item)
        {
            base.AddChild(item);
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
                result.Load(LoadRecursive(folder));
            foreach (var file in rootDirectory.GetFiles())
                result.Load(new FileSystemTreeItem() { Name = file.Name, FullPath = file.FullName, IsFolder = false });

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
