using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public interface IExplorerProvider
    {
        IEnumerable<Type> ItemTypes { get; }
        bool IsOpened { get; }
        string SourceName { get; }
        TreeItem SourceTree { get; }
        TreeItem OpenSource();
        void CloseSource();
        TreeItem CreateItem(string fullPath, string name, EditorFileTemplate fileTemplate);
        FolderTreeItem CreateFolder(string fullPath, string name);
        void UpdateItem(string fullPath, string newName);
        void MoveItem(string fullPath, string newFullPath);
        void DeleteItem(string fullPath);
    }
}
