using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace Gemini.Modules.Explorer.Models
{
    public abstract class TreeItem : PropertyChangedBase
    {
        private readonly BindableCollection<TreeItem> _children = new BindableCollection<TreeItem>();

        public abstract string Name { get; set; }
        public abstract string FullPath { get; set; }
        public abstract Uri IconSource { get; }
        public abstract bool CanOpenDocument { get; }
        public Guid DocumentId { get; set; }
        public IReadOnlyList<TreeItem> Children => _children;

        public virtual void AddChild(TreeItem item)
        {
            _children.Add(item);
        }
        public virtual void RemoveChild(TreeItem item)
        {
            _children.Remove(item);
        }
        public virtual TreeItem FindChildRecursive(TreeItem item)
        {
            return FindChildRecursive(item.FullPath, this);
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
    }
}
