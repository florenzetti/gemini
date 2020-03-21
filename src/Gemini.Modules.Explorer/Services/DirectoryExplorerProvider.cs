using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows.Forms;

namespace Gemini.Modules.Explorer.Services
{
    [Export(typeof(IExplorerProvider))]
    public class DirectoryExplorerProvider : IExplorerProvider, IDisposable
    {
        private FileSystemWatcher _fsWatcher;
        private DirectoryInfo _directoryInfo;

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
                yield return typeof(FileSystemFolderTreeItem);
                yield return typeof(FileSystemFileTreeItem);
            }
        }

        public bool IsOpened => SourceTree != null;
        public string SourceName => _directoryInfo?.Name;
        public TreeItem SourceTree { get; private set; }

        public TreeItem Open()
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _directoryInfo = new DirectoryInfo(folderDialog.SelectedPath);
                CreateFileSystemWacther(folderDialog.SelectedPath);
                SourceTree = FileSystemFolderTreeItem.LoadRecursive(_directoryInfo);
            }
            return SourceTree;
        }

        public void Dispose()
        {
            _directoryInfo = null;
            SourceTree = null;
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
            item.Parent.RemoveChild(item);
        }

        private void OnFileSystemRenamed(object sender, RenamedEventArgs e)
        {
            var item = SourceTree.FindChildRecursive(e.OldFullPath);
            item.Name = Path.GetFileName(e.Name);
            item.FullPath = e.FullPath;
        }

        private void OnFileSystemCreated(object sender, FileSystemEventArgs e)
        {
            TreeItem item;
            var attributes = File.GetAttributes(e.FullPath);
            if (attributes.HasFlag(FileAttributes.Directory))
                item = FileSystemFolderTreeItem.LoadRecursive(new DirectoryInfo(e.FullPath));
            else
                item = new FileSystemFileTreeItem(Path.GetFileName(e.Name), e.FullPath);

            var parentDirectoryItem = SourceTree.FindChildRecursive(Path.GetDirectoryName(e.FullPath));
            parentDirectoryItem.LoadChild(item);
        }

        public void Close()
        {
            Dispose();
        }
    }
}
