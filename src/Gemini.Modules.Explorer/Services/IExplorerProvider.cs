using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public interface IExplorerProvider
    {
        //event ExplorerItemChangedEventHandler ItemCreated;
        //event ExplorerItemRenamedEventHandler ItemRenamed;
        //event ExplorerItemChangedEventHandler ItemDeleted;

        bool EnableRaisingEvents { get; set; }
        IEnumerable<Type> ItemTypes { get; }
        bool IsOpened { get; }
        string SourceName { get; }
        TreeItem SourceTree { get; }
        TreeItem Open();
        void Close();
    }
}
