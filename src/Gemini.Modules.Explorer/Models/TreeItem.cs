using Caliburn.Micro;
using Gemini.Modules.Explorer.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Gemini.Modules.Explorer.Models
{
    public class TreeItem : PropertyChangedBase
    {
        public Guid DocumentId { get; set; }
        public string Name { get; set; }
        public string FullPath { get; set; }
        public Uri IconSource => GetIconSource();
        public bool IsFolder { get; private set; }
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
        public IList<TreeItem> Children { get; }

        public TreeItem()
        {
            Children = new List<TreeItem>();
        }

        public static TreeItem LoadRecursive(DirectoryInfo rootDirectory)
        {
            var result = new TreeItem()
            {
                Name = rootDirectory.Name,
                FullPath = rootDirectory.FullName,
                IsFolder = true,
                IsExpanded = false
            };

            foreach (var folder in rootDirectory.GetDirectories())
                result.Children.Add(LoadRecursive(folder));
            foreach (var file in rootDirectory.GetFiles())
                result.Children.Add(new TreeItem() { Name = file.Name, FullPath = file.FullName, IsFolder = false });

            return result;
        }

        private Uri GetIconSource()
        {
            Uri icon = null;
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
