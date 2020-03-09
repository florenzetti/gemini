using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Gemini.Modules.Explorer.Services
{
    [Export(typeof(IExplorerProvider))]
    public class DirectoryExplorerProvider : IExplorerProvider, IDisposable
    {
        private FileSystemWatcher _fsWatcher;

        private DirectoryInfo _directoryInfo;
        public bool IsOpened => _directoryInfo != null;
        public string SourceName => _directoryInfo?.Name;

        private event ExplorerItemChangedEventHandler _itemCreated;
        private event ExplorerItemRenamedEventHandler _itemRenamed;
        private event ExplorerItemChangedEventHandler _itemDeleted;

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
            FileSystemTreeItem result = null;
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _directoryInfo = new DirectoryInfo(folderDialog.SelectedPath);
                CreateFileSystemWacther(folderDialog.SelectedPath);
                result = FileSystemTreeItem.LoadRecursive(_directoryInfo);
            }
            return result;
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
            var args = new ExplorerItemChangedEventArgs(e.FullPath, e.Name, ExplorerItemChangeType.Deleted);
            _itemDeleted?.Invoke(this, args);
        }

        private void OnFileSystemRenamed(object sender, RenamedEventArgs e)
        {
            var args = new ExplorerItemRenamedEventArgs(e.FullPath, e.Name, e.OldFullPath, e.OldName);
            _itemRenamed?.Invoke(this, args);
        }

        private void OnFileSystemCreated(object sender, FileSystemEventArgs e)
        {
            var args = new ExplorerItemChangedEventArgs(e.FullPath, e.Name, ExplorerItemChangeType.Created);
            _itemCreated?.Invoke(this, args);
        }

        public void Close()
        {
            Dispose();
        }
    }
}
