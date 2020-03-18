using Gemini.Framework.Commands;
using Gemini.Framework.Threading;
using Gemini.Modules.Explorer.Commands;
using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gemini.Modules.Explorer.Services
{
    [Export(typeof(IExplorerProvider))]
    public class DirectoryExplorerProvider : IExplorerProvider, IDisposable
    {
        private FileSystemWatcher _fsWatcher;
        private DirectoryInfo _directoryInfo;

        private event ExplorerItemChangedEventHandler _itemCreated;
        private event ExplorerItemRenamedEventHandler _itemRenamed;
        private event ExplorerItemChangedEventHandler _itemDeleted;

        public bool EnableRaisingEvents
        {
            get
            {
                return _fsWatcher?.EnableRaisingEvents ?? false;
            }
            set
            {
                if (_fsWatcher != null)
                    _fsWatcher.EnableRaisingEvents = value;
            }
        }

        public IEnumerable<Type> ItemTypes
        {
            get
            {
                yield return typeof(DirectoryTreeItem);
                yield return typeof(FileSystemFileTreeItem);
            }
        }

        public bool IsOpened => SourceTree != null;
        public string SourceName => _directoryInfo?.Name;
        public TreeItem SourceTree { get; private set; }

        event ExplorerItemChangedEventHandler IExplorerProvider.ItemCreated
        {
            add { _itemCreated += value; }
            remove { _itemCreated -= value; }
        }

        event ExplorerItemRenamedEventHandler IExplorerProvider.ItemRenamed
        {
            add { _itemRenamed += value; }
            remove { _itemRenamed -= value; }
        }

        event ExplorerItemChangedEventHandler IExplorerProvider.ItemDeleted
        {
            add { _itemDeleted += value; }
            remove { _itemDeleted -= value; }
        }

        public TreeItem Open()
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _directoryInfo = new DirectoryInfo(folderDialog.SelectedPath);
                CreateFileSystemWacther(folderDialog.SelectedPath);
                SourceTree = DirectoryTreeItem.LoadRecursive(_directoryInfo);
            }
            return SourceTree;
        }

        public void Dispose()
        {
            _directoryInfo = null;
            _fsWatcher?.Dispose();
            _fsWatcher = null;
        }

        private void CreateFileSystemWacther(string folderFullPath)
        {
            _fsWatcher = new FileSystemWatcher()
            {
                Path = folderFullPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes | NotifyFilters.DirectoryName,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };
            _fsWatcher.Created += OnFileSystemCreated;
            _fsWatcher.Renamed += OnFileSystemRenamed;
            _fsWatcher.Deleted += OnFileSystemDeleted;
        }

        private void OnFileSystemDeleted(object sender, FileSystemEventArgs e)
        {
            var item = SourceTree.FindChildRecursive(e.FullPath);
            var args = new ExplorerItemChangedEventArgs(item, ExplorerItemChangeType.Deleted);
            _itemDeleted?.Invoke(this, args);
        }

        private void OnFileSystemRenamed(object sender, RenamedEventArgs e)
        {
            var item = SourceTree.FindChildRecursive(e.OldFullPath);
            var args = new ExplorerItemRenamedEventArgs(e.Name, e.FullPath, item);
            _itemRenamed?.Invoke(this, args);
        }

        private void OnFileSystemCreated(object sender, FileSystemEventArgs e)
        {
            TreeItem item;
            var attributes = File.GetAttributes(e.FullPath);
            if (attributes.HasFlag(FileAttributes.Directory))
                item = new DirectoryTreeItem();
            else
                item = new FileSystemFileTreeItem();

            item.FullPath = e.FullPath;
            item.Name = e.Name;

            var args = new ExplorerItemChangedEventArgs(item, ExplorerItemChangeType.Created);
            _itemCreated?.Invoke(this, args);
        }

        public void Close()
        {
            Dispose();
        }
    }
}
