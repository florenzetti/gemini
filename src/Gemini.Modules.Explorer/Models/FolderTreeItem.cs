using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gemini.Modules.Explorer.Models
{
    public abstract class FolderTreeItem : TreeItem
    {
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
        public override bool CanOpenDocument => false;
    }
}
