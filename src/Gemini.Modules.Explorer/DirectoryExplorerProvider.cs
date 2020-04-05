using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Models;
using Gemini.Modules.Explorer.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Gemini.Modules.Explorer.Services
{
    [Export(typeof(IExplorerProvider))]
    public class DirectoryExplorerProvider : IExplorerProvider, IDisposable
    {
        private FileSystemWatcher _fsWatcher;
        private DirectoryInfo _directoryInfo;

        private readonly List<EditorFileTemplate> _itemTemplates;
        public IEnumerable<EditorFileTemplate> ItemTemplates => _itemTemplates;

        public bool IsOpened => SourceTree != null;
        public string SourceName => Resources.FolderText;
        public TreeItem SourceTree { get; private set; }

        public DirectoryExplorerProvider()
        {
            _itemTemplates = new List<EditorFileTemplate>
            {
                DefaultFileTemplate.DefaultTemplate,
                DefaultFolderTemplate.DefaultTemplate
            };
        }

        public TreeItem OpenSource()
        {
            var folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                _directoryInfo = new DirectoryInfo(folderDialog.SelectedPath);
                CreateFileSystemWacther(folderDialog.SelectedPath);
                SourceTree = LoadRecursive(_directoryInfo, true);
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
                item = LoadRecursive(new DirectoryInfo(e.FullPath));
            else
                item = new TreeItem(e.FullPath, Path.GetFileName(e.Name));

            var parentDirectoryItem = SourceTree.FindChildRecursive(Path.GetDirectoryName(e.FullPath));
            parentDirectoryItem.LoadChild(item);
        }

        public void CloseSource()
        {
            Dispose();
        }

        public EditorFileTemplate GetTemplate(TreeItem item)
        {
            if (item is FolderTreeItem)
                return _itemTemplates.First(o => o is DefaultFolderTemplate);
            else
            {
                var template = _itemTemplates.FirstOrDefault(o => o.FileExtension == Path.GetExtension(item.FullPath));
                return template ?? _itemTemplates.First(o => o is DefaultFileTemplate);
            }
        }

        public TreeItem CreateItem(string fullPath, string name, EditorFileTemplate editorFileTemplate)
        {
            if (!IsOpened)
                return default;

            _fsWatcher.EnableRaisingEvents = false;
            using (var writer = File.CreateText(fullPath))
            {
                writer.Write(editorFileTemplate.InitContent);
            }
            _fsWatcher.EnableRaisingEvents = true;

            return new TreeItem(fullPath, name);
        }

        public FolderTreeItem CreateFolder(string fullPath, string name)
        {
            if (!IsOpened)
                return default;

            _fsWatcher.EnableRaisingEvents = false;
            Directory.CreateDirectory(fullPath);
            _fsWatcher.EnableRaisingEvents = true;

            return new FolderTreeItem(fullPath, name);
        }

        public void UpdateItem(string fullPath, string newName)
        {
            var newFullPath = Path.Combine(Path.GetDirectoryName(fullPath), newName);
            MoveItem(fullPath, newFullPath);
        }

        public void MoveItem(string fullPath, string newFullPath)
        {
            if (!IsOpened || fullPath == newFullPath)
                return;
            _fsWatcher.EnableRaisingEvents = false;
            if (File.Exists(fullPath))
                File.Move(fullPath, newFullPath);
            else if (Directory.Exists(fullPath))
                Directory.Move(fullPath, newFullPath);
            _fsWatcher.EnableRaisingEvents = true;
        }

        public void DeleteItem(string fullPath)
        {
            if (!IsOpened)
                return;

            _fsWatcher.EnableRaisingEvents = false;
            if (Directory.Exists(fullPath))
                Directory.Delete(fullPath, true);
            else if (File.Exists(fullPath))
                File.Delete(fullPath);
            _fsWatcher.EnableRaisingEvents = true;
        }

        private static FolderTreeItem LoadRecursive(DirectoryInfo rootDirectory)
        {
            return LoadRecursive(rootDirectory, false);
        }

        private static FolderTreeItem LoadRecursive(DirectoryInfo rootDirectory, bool isRootExpanded)
        {
            var result = new FolderTreeItem(rootDirectory.FullName, rootDirectory.Name)
            {
                IsExpanded = isRootExpanded
            };

            foreach (var folder in rootDirectory.GetDirectories())
                result.LoadChild(LoadRecursive(folder, false));
            foreach (var file in rootDirectory.GetFiles())
                result.LoadChild(new TreeItem(file.FullName, file.Name));

            return result;
        }
    }
}
