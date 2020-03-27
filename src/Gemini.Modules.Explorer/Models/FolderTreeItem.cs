using System;

namespace Gemini.Modules.Explorer.Models
{
    public class FolderTreeItem : TreeItem
    {
        public virtual bool IsRootFolder { get; }

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
        public override bool CanOpenDocument => false;

        public override Uri IconSource
        {
            get
            {
                Uri icon;
                if (IsExpanded)
                    icon = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/folder_open.png");
                else
                    icon = new Uri("pack://application:,,,/Gemini.Modules.Explorer;component/Resources/Icons/folder.png");
                return icon;
            }
        }

        public FolderTreeItem(string fullPath, string name) : base(fullPath, name)
        {
        }
    }
}
