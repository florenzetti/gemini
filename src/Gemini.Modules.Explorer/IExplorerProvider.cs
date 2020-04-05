using System.Collections.Generic;
using Gemini.Framework.Services;
using Gemini.Modules.Explorer.Models;

namespace Gemini.Modules.Explorer
{
    public interface IExplorerProvider
    {
        IEnumerable<EditorFileTemplate> ItemTemplates { get; }
        bool IsOpened { get; }
        string SourceName { get; }
        TreeItem SourceTree { get; }
        TreeItem OpenSource();
        void CloseSource();
        EditorFileTemplate GetTemplate(TreeItem item);
        TreeItem CreateItem(string fullPath, string name, EditorFileTemplate fileTemplate);
        FolderTreeItem CreateFolder(string fullPath, string name);
        void UpdateItem(string fullPath, string newName);
        void MoveItem(string fullPath, string newFullPath);
        void DeleteItem(string fullPath);
    }
}
