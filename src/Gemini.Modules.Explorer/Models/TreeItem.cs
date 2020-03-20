using Caliburn.Micro;
using Gemini.Modules.Explorer.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Gemini.Modules.Explorer.Models
{
    public abstract class TreeItem : PropertyChangedBase
    {
        private readonly BindableCollection<TreeItem> _children = new BindableCollection<TreeItem>();

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        private string _fullPath;
        public string FullPath
        {
            get => _fullPath;
            set
            {
                _fullPath = value;
                NotifyOfPropertyChange(() => FullPath);
            }
        }
        private bool _isEditing;
        public virtual bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                NotifyOfPropertyChange(() => IsEditing);
            }
        }
        public abstract Uri IconSource { get; }
        public abstract bool CanOpenDocument { get; }
        public Guid DocumentId { get; set; }
        public TreeItem Parent { get; private set; }
        public IReadOnlyList<TreeItem> Children => _children;

        public virtual void LoadChild(TreeItem item)
        {
            item.Parent = this;
            _children.Add(item);
        }

        public virtual void AddChild(TreeItem item)
        {
            LoadChild(item);
        }
        public virtual void RemoveChild(TreeItem item)
        {
            if (_children.Contains(item))
            {
                item.Parent = null;
                _children.Remove(item);
            }
        }
        public virtual void MoveTo(TreeItem parent)
        {
            Parent.RemoveChild(this);
            Parent = parent;
            Parent.AddChild(this);
        }
        public virtual TreeItem FindChildRecursive(string fullPath)
        {
            return FindChildRecursive(fullPath, this);
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
                        result = FindChildRecursive(fullPath, folder);
                        if (result != null)
                            break;
                    }
                }
            }
            return result;
        }
    }
}
