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

        public IEnumerable<Type> ItemTypes
        {
            get
            {
                yield return typeof(FileSystemTreeItem);
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
                SourceTree = FileSystemTreeItem.LoadRecursive(_directoryInfo);
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
            var item = new FileSystemTreeItem()
            {
                FullPath = e.FullPath,
                Name = e.Name
            };
            var args = new ExplorerItemChangedEventArgs(item, ExplorerItemChangeType.Deleted);
            _itemDeleted?.Invoke(this, args);
        }

        private void OnFileSystemRenamed(object sender, RenamedEventArgs e)
        {
            var item = new FileSystemTreeItem()
            {
                FullPath = e.FullPath,
                Name = e.Name
            };
            var oldItem = new FileSystemTreeItem()
            {
                FullPath = e.OldFullPath,
                Name = e.OldName
            };
            var args = new ExplorerItemRenamedEventArgs(item, oldItem);
            _itemRenamed?.Invoke(this, args);
        }

        private void OnFileSystemCreated(object sender, FileSystemEventArgs e)
        {
            var item = new FileSystemTreeItem()
            {
                FullPath = e.FullPath,
                Name = e.Name
            };
            var args = new ExplorerItemChangedEventArgs(item, ExplorerItemChangeType.Created);
            _itemCreated?.Invoke(this, args);
        }

        public void Close()
        {
            Dispose();
        }
    }
}
