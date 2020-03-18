using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public class ExplorerItemRenamedEventArgs : ExplorerItemChangedEventArgs
    {
        public string NewName { get; }
        public string NewFullPath { get; }
        internal ExplorerItemRenamedEventArgs(string newName, string newFullPath, TreeItem oldItem)
            : base(oldItem, ExplorerItemChangeType.Renamed)
        {
            NewName = newName;
            NewFullPath = newFullPath;
        }
    }

    public delegate void ExplorerItemRenamedEventHandler(object sender, ExplorerItemRenamedEventArgs e);
}
