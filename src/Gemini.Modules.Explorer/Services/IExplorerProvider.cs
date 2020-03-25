using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public interface IExplorerProvider
    {
        bool EnableRaisingEvents { get; set; }
        IEnumerable<Type> ItemTypes { get; }
        bool IsOpened { get; }
        string SourceName { get; }
        TreeItem SourceTree { get; }
        TreeItem Open();
        TreeItem CreateItem(string name, string fullPath, EditorFileTemplate fileTemplate);
        void Close();
    }
}
