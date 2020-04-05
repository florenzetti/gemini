using Caliburn.Micro;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Gemini.Modules.Explorer.Models
{
    public class TreeItem : PropertyChangedBase, IEnumerable<TreeItem>
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
        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }
        public IEnumerable<TreeItem> AllSelectedItems
        {
            get => GetAllRecursive().Where(o => o.IsSelected);
        }
        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                NotifyOfPropertyChange(() => IsEditing);
            }
        }
        public virtual Uri IconSource => IconSourceUtility.GetByExtension(Path.GetExtension(Name), 16);
        public virtual bool CanOpenDocument => true;
        private Visibility _visibility;
        public Visibility Visibility
        {
            get => _visibility;
            private set
            {
                _visibility = value;
                NotifyOfPropertyChange(() => Visibility);
            }
        }
        public Guid DocumentId { get; set; }
        public TreeItem Parent { get; private set; }
        public IReadOnlyList<TreeItem> Children => _children;

        public TreeItem(string fullPath, string name)
        {
            FullPath = fullPath;
            Name = name;
        }

        public void LoadChild(TreeItem item)
        {
            item.Parent = this;
            _children.Add(item);
        }
        public void UnloadChild(TreeItem item)
        {
            _children.Remove(item);
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
        public virtual void MoveTo(TreeItem moveToItem)
        {
            if (moveToItem == this
                || FindChildRecursive(moveToItem.FullPath) != null) //trying to move to a child folder
                return;

            Parent.UnloadChild(this);
            Parent = moveToItem;
            FullPath = Path.Combine(Parent.FullPath, Name);
            Parent.LoadChild(this);
        }
        public virtual TreeItem FindChildRecursive(string fullPath)
        {
            return FindChildRecursive(fullPath, this);
        }
        public void Search(string searchTerm)
        {
            SearchClearRecursive(this);
            if (!string.IsNullOrWhiteSpace(searchTerm) && searchTerm.Length >= 2)
            {
                foreach (var item in Children.Where(x => !x.Name.ToUpper().Contains(searchTerm.ToUpper()) && !x.Name.ToUpper().Equals(searchTerm.ToUpper())))
                    SearchRecursive(item, searchTerm);
            }
        }
        private void SearchRecursive(TreeItem rootItem, string searchTerm)
        {
            foreach (var item in rootItem.Children)
            {
                SearchRecursive(item, searchTerm);
            }
            if ((!rootItem.Name.ToUpper().Contains(searchTerm.ToUpper()) && !rootItem.Name.ToUpper().Equals(searchTerm.ToUpper()))
                && !rootItem.Children.Any(o => o.Visibility == Visibility.Visible))
            {
                rootItem.Visibility = Visibility.Collapsed;
            }
        }

        private void SearchClearRecursive(TreeItem rootItem)
        {
            rootItem.Visibility = Visibility.Visible;
            foreach (var item in rootItem.Children)
            {
                SearchClearRecursive(item);
            }
        }

        public IEnumerable<TreeItem> GetAllRecursive()
        {
            return GetAllRecursive(this);
        }

        protected static IEnumerable<TreeItem> GetAllRecursive(TreeItem item)
        {
            var result = new List<TreeItem>();
            result.Add(item);
            foreach (var child in item.Children)
                result.AddRange(GetAllRecursive(child));
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
                        result = FindChildRecursive(fullPath, folder);
                        if (result != null)
                            break;
                    }
                }
            }
            return result;
        }

        public IEnumerator<TreeItem> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
