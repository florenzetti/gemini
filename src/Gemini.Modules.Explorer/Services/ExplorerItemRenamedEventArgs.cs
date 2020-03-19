using Gemini.Modules.Explorer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gemini.Modules.Explorer.Services
{
    public class ExplorerItemRenamedEventArgs : ExplorerItemChangedEventArgs
    {
        public string OldName { get; }
        public string OldFullPath { get; }
        internal ExplorerItemRenamedEventArgs(TreeItem item, string oldName, string oldFullPath)
            : base(item, ExplorerItemChangeType.Renamed)
        {
            OldName = oldName;
            OldFullPath = oldFullPath;
        }
    }

    public delegate void ExplorerItemRenamedEventHandler(object sender, ExplorerItemRenamedEventArgs e);
}
